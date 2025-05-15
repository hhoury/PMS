using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PMS.Application.Models;
using PMS.Application.Services;

namespace PMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly ILogger<AuthenticateController> _logger;

        public AuthenticateController(IAuthenticationService authService, ILogger<AuthenticateController> logger)
        {
            _authService = authService;
            _logger = logger;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest model)
        {
            _logger.LogInformation($"Login attempt for user {model.Email}");
            var result = await _authService.LoginAsync(model);
            if (result.Token != null)
            {
                _logger.LogInformation($"{model.Email} logged in successfully");
                return Ok(new
                {
                    Token = result.Token,
                    RefreshToken = result.RefreshToken,
                    Expiration = result.Expiration
                });
            }
            _logger.LogWarning($"Login failed for {model.Email}");
            return Unauthorized();
        }

        [Authorize]
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            _logger.LogInformation($"Registering {model.Email}");
            var result = await _authService.RegisterAsync(model);
            if (result)
            {
                _logger.LogInformation($"Admin {model.Email} registered successfully");
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
            _logger.LogError($"Registering new {model.Email} Failed!");
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists or creation failed!" });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            _logger.LogInformation("Refreshing token");
            var result = await _authService.RefreshTokenAsync(tokenModel);
            if (result.AccessToken != null)
            {
                _logger.LogInformation("Token refreshed successfully");
                return new ObjectResult(new
                {
                    accessToken = result.AccessToken,
                    refreshToken = result.RefreshToken
                });
            }
            _logger.LogWarning("Token refresh failed");
            return BadRequest("Invalid access token or refresh token");
        }

        [Authorize]
        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> Revoke([FromBody] string Email)
        {
            _logger.LogInformation($"Revoking token for user {Email}");
            var result = await _authService.RevokeAsync(Email);
            if (result)
            {
                _logger.LogInformation($"Token revoked for user {Email}");
                return NoContent();
            }
            _logger.LogWarning($"Token revocation failed for user {Email}");
            return BadRequest("Invalid user name");
        }

        [Authorize]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            _logger.LogInformation("Revoking tokens for all users");
            await _authService.RevokeAllAsync();
            _logger.LogInformation("Tokens revoked for all users");
            return NoContent();
        }
        [Authorize]
        [HttpPost]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest model)
        {
            _logger.LogInformation($"Changing password for user {model.Email}");
            var result = await _authService.ChangePasswordAsync(model);
            if (result)
            {
                _logger.LogInformation($"Password changed successfully for user {model.Email}");
                return Ok(new Response { Status = "Success", Message = "Password changed successfully!" });
            }
            _logger.LogError($"Password change failed for user {model.Email}");
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password change failed!" });
        }

        [Authorize]
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            _logger.LogInformation($"Resetting password for user {model.Email}");
            var result = await _authService.ResetPasswordAsync(model);
            if (result)
            {
                _logger.LogInformation($"Password reset successfully for user {model.Email}");
                return Ok(new Response { Status = "Success", Message = "Password reset successfully!" });
            }
            _logger.LogError($"Password reset failed for user {model.Email}");
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Password reset failed!" });
        }

    }
}

using PMS.Application.Models;

namespace PMS.Application.Services
{
    public interface IAuthenticationService
    {
        Task<(string Token, string RefreshToken, DateTime Expiration)> LoginAsync(AuthRequest model);
        Task<bool> RegisterAsync(RegisterModel model);
        Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(TokenModel tokenModel);
        Task<bool> RevokeAsync(string username);
        Task RevokeAllAsync();
        Task<bool> ChangePasswordAsync(ChangePasswordRequest model);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
    }
}

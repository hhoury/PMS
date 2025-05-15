using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyMembersController : ControllerBase
    {
        private readonly ILogger<PolicyMembersController> _logger;
        private readonly IPolicyMemberRepository _policyService;

        public PolicyMembersController(ILogger<PolicyMembersController> logger, IPolicyMemberRepository policyService)
        {
            _logger = logger;
            _policyService = policyService;
        }

        [HttpPost("CreatePolicyMember")]
        public IActionResult AddPolicyMember([FromBody] CreatePolicyMember request)
        public IActionResult AddPolicyMember([FromBody] CreatePolicyMemberDto request)
        {
            try
            {
                _logger.LogInformation("Adding member to policy ID: {PolicyId}", request.PolicyId);
                var result = _policyService.AddPolicyMember(request.PolicyId, request.Member);
                if (!result)
                {
                    _logger.LogWarning("Failed to add member to policy ID: {PolicyId}", request.PolicyId);
                    return NotFound(new { message = "Policy not found or member could not be added." });
                }
                return Ok(new { message = "Member added to policy successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding member to policy ID: {PolicyId}", request.PolicyId);
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while adding the member to the policy." });
            }
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace PMS.Application.Models
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters", MinimumLength = 8)]
        public string Password { get; set; }
    }
}

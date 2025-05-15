using System.ComponentModel.DataAnnotations;

namespace PMS.Application.Models
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(15, ErrorMessage = "Your Password is limited to {2} to {1} characters", MinimumLength = 8)]
        public string NewPassword { get; set; }
    }
}

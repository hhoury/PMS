using Microsoft.AspNetCore.Identity;

namespace PMS.Domain.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? LastModifiedDate { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsDeleted { get; set; }

    }
}

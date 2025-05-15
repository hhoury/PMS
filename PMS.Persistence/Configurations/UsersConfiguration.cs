using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;

namespace PMS.Persistence.Configurations
{
    public class UsersConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var creationDate = new DateTime(2025, 1, 1);

            var hasher = new PasswordHasher<ApplicationUser>();
            ApplicationUser admin = new ApplicationUser
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "Admin",
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                PhoneNumber = "123456789",
                Email = "admin@localhost.com",
                EmailConfirmed = true,
                DateCreated = creationDate,
                NormalizedEmail = "ADMIN@LOCALHOST.COM",
                PasswordHash = hasher.HashPassword(null, "P@ssw0rd@123"),
                SecurityStamp = "asdjbas@ASDH#%asdsada23132asd",
            };
        }
    }
}

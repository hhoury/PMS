using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMS.Domain.Entities;
using PMS.Persistence.Configurations;
using System.Security.Claims;

namespace PMS.Persistence.DatabaseContext
{
    public class PMSDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PMSDbContext(DbContextOptions<PMSDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<Policy> Policies { get; set; }
        public DbSet<PolicyMember> PolicyMembers { get; set; }
        public DbSet<Member> Members { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                entry.Entity.DateModified = DateTime.UtcNow;
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
                        entry.Entity.DateCreated = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.ModifiedBy = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
                        entry.Entity.DateModified = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.DeletedBy = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DateDeleted = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Policy>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<PolicyMember>().HasQueryFilter(p => !p.IsDeleted);
            modelBuilder.Entity<Member>().HasQueryFilter(p => !p.IsDeleted);

            modelBuilder.ApplyConfiguration(new UsersConfiguration());

        }
    }
}

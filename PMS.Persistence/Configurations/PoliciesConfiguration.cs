using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Persistence.Configurations
{
    public class PoliciesConfiguration : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.HasData(
                new Policy
                {
                    Id = 1,
                    Name = "Health Policy",
                    Description = "Health insurance policy",
                    DateCreated = DateTime.UtcNow,
                    PolicyType = Domain.Enumeration.PolicyType.HealthInsurance,
                },
                new Policy
                {
                    Id = 2,
                    Name = "Travel Policy",
                    Description = "Travel insurance policy",
                    DateCreated = DateTime.UtcNow,
                    PolicyType = Domain.Enumeration.PolicyType.TravelInsurance
                },
                new Policy
                {
                    Id = 3,
                    Name = "Automotive Policy",
                    Description = "Automotive insurance policy",
                    DateCreated = DateTime.UtcNow,
                    PolicyType = Domain.Enumeration.PolicyType.AutoInsurance
                }
            );
        }
    }
}

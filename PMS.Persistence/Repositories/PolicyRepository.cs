using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PMS.Application.Contracts;
using PMS.Application.DTOs.Policies;
using PMS.Domain.Entities;
using PMS.Domain.Enumeration;
using PMS.Persistence.DatabaseContext;

namespace PMS.Persistence.Repositories
{
    public class PolicyRepository : GenericRepository<Policy>, IPolicyRepository
    {
        private readonly PMSDbContext _context;
        public PolicyRepository(PMSDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<List<Policy>> GetByPolicyTypeAsync(PolicyType type)
        {
            var policies = await _context.Set<Policy>()
                .Where(p => p.PolicyType == type && !p.IsDeleted)
                .AsNoTracking()
                .ToListAsync();

            return policies;
        }
    }
}

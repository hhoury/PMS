using AutoMapper;
using PMS.Application.Contracts;
using PMS.Domain.Entities;
using PMS.Persistence.DatabaseContext;

namespace PMS.Persistence.Repositories
{
    public class PolicyMemberRepository : GenericRepository<PolicyMember>, IPolicyMemberRepository
    {
        public PolicyMemberRepository(PMSDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}

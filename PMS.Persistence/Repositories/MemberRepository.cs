using AutoMapper;
using PMS.Application.Contracts;
using PMS.Domain.Entities;
using PMS.Persistence.DatabaseContext;

namespace PMS.Persistence.Repositories
{
    public class MemberRepository : GenericRepository<Member>, IMemberRepository
    {
        public MemberRepository(PMSDbContext context, IMapper mapper) : base(context, mapper)
        {
        }
    }
}

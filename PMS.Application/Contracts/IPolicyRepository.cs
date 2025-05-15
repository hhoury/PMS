using PMS.Application.DTOs.Policies;
using PMS.Domain.Entities;
using PMS.Domain.Enumeration;

namespace PMS.Application.Contracts
{
    public interface IPolicyRepository : IGenericRepository<Policy>
    {
        Task<List<Policy>> GetByPolicyTypeAsync(PolicyType type);
    }
}

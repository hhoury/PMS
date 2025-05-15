namespace PMS.Application.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IPolicyRepository Policies { get; } // Repository for Policy entity
        IMemberRepository Members { get; } // Repository for Memberships entity
        IPolicyMemberRepository PolicyMembers { get; } // Repository for PolicyMember entity
        Task<int> CompleteAsync(); // Save changes
    }
}

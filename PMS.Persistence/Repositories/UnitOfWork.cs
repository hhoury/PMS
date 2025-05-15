using Microsoft.Extensions.DependencyInjection;
using PMS.Application.Contracts;
using PMS.Persistence.DatabaseContext;

namespace PMS.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly PMSDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private bool _disposed = false;

        public UnitOfWork(PMSDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IPolicyRepository Policies => _serviceProvider.GetRequiredService<IPolicyRepository>();
        public IMemberRepository Members => _serviceProvider.GetRequiredService<IMemberRepository>();
        public IPolicyMemberRepository PolicyMembers => _serviceProvider.GetRequiredService<IPolicyMemberRepository>();

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}

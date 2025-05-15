using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PMS.Application.Contracts;
using PMS.Application.Services;
using PMS.Persistence.DatabaseContext;
using PMS.Persistence.Repositories;
using PMS.Persistence.Services;

namespace PMS.Persistence
{
    public static class PersistenceServicesRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PMSDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("PMS_ConnectionString"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            return services;
        }
    }
}

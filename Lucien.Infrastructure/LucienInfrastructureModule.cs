using Lucien.Domain;
using Lucien.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lucien.Infrastructure
{
    public class LucienInfrastructureModule
    {
        public static void PreConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            RegisterDbContext(services, configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Register MediatR (include assemblies that have your handlers)
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssemblies(
                    typeof(LucienInfrastructureModule).Assembly, // Infrastructure
                    typeof(LucienDomainModule).Assembly           // Domain/Application (handlers/events)
                ));

            LucienDomainModule.PreConfigureServices(services, configuration);
        }

        private static void RegisterDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LucienDbContext>(options =>
                options.UseNpgsql(GetConnectionString(configuration)));
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString(ConnectionStringsConst.DefaultConnection);
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string is missing");
            }

            return connectionString;
        }
    }
}

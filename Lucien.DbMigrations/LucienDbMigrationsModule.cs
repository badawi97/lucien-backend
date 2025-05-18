using Lucien.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lucien.DbMigrations
{
    public class LucienDbMigrationsModule
    {
        public static void PreConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            LucienInfrastructureModule.PreConfigureServices(services, configuration);
        }

        private static void RegisterDbContext(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LucienDbContext>(options =>
                options.UseNpgsql(GetConnectionString(configuration)));
        }

        private static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Connection string is missing");
            }

            return connectionString;
        }
    }
}

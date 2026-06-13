using Lucien.Infrastructure;
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
    }
}

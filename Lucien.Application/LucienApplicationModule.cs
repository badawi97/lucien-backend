using Lucien.Application.Contracts;
using Lucien.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lucien.Application
{
    public class LucienApplicationModule
    {
        public static void PreConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            LucienDomainModule.PreConfigureServices(services, configuration);
            LucienApplicationContractsModule.PreConfigureServices(services, configuration);
            services.AddAutoMapper(typeof(LucienApplicationAutoMapperProfile).Assembly);
        }
    }
}

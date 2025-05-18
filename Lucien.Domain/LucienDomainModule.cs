using Lucien.Domain.Shared;
using Lucien.Domain.Shared.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Lucien.Domain
{
    public class LucienDomainModule
    {
        public static void PreConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            LucienDmainSharedModule.PreConfigureServices(services, configuration);
            var assemblies = GetApplicationServiceAssemblies();
            RegisterTransientServices(services, assemblies);
            RegisterSingletonServices(services, assemblies);
            RegisterScopedServices(services, assemblies);
        }

        private static void RegisterTransientServices(IServiceCollection services, Assembly[] assemblies)
        {
            // Get all assemblies loaded in the app domain

            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<ITransient>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        }

        private static void RegisterSingletonServices(IServiceCollection services, Assembly[] assemblies)
        {
            services.Scan(scan => scan
                .FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo<ISingleton>())
                .AsImplementedInterfaces()
                .WithSingletonLifetime());
        }

        private static void RegisterScopedServices(IServiceCollection services, Assembly[] assemblies)
        {
            services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo<IScoped>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());
        }

        private static Assembly[] GetApplicationServiceAssemblies()
        {
            // Get all assemblies loaded in the app domain that contain types ending with "ApplicationService"
            return AppDomain.CurrentDomain.GetAssemblies();
        }

        private static bool AssemblyHasApplicationServiceTypes(Assembly assembly)
        {
            try
            {
                // Check if the assembly contains any type ending with "ApplicationService"
                return assembly.GetTypes().Any(t => t.Name.EndsWith("ApplicationService"));
            }
            catch
            {
                // In case of any error (like assembly not being fully loaded), ignore the assembly
                return false;
            }
        }
    }
}

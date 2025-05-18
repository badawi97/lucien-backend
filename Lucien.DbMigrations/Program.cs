using Lucien.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lucien.DbMigrations
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args);

            var host = builder.Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                await ApplyMigrationsAsync(serviceProvider);
                SeedDatabase(serviceProvider);
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                 Host.CreateDefaultBuilder(args)
                     .ConfigureServices((context, services) =>
                     {
                         var configuration = context.Configuration;
                         LucienInfrastructureModule.PreConfigureServices(services, configuration);

                         services.AddScoped<DbInitializer>();
                     });

        private static async Task ApplyMigrationsAsync(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<LucienDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        private static void SeedDatabase(IServiceProvider serviceProvider)
        {
            var dbInitializer = serviceProvider.GetRequiredService<DbInitializer>();
            dbInitializer.Seed();
        }

    }
}

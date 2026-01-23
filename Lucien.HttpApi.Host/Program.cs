namespace Lucien.HttpApi.Host
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.ConfigureKestrel(options =>
            {
                var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
                options.ListenAnyIP(int.Parse(port));
            });

            var configuration = builder.Configuration;

            LucienHttpApiHostModule.PreConfigureServices(
                builder.Services,
                builder.Logging,
                configuration
            );

            var app = builder.Build();

            LucienHttpApiHostModule.ConfigureServices(app, configuration);

            await app.RunAsync();

        }
    }
}

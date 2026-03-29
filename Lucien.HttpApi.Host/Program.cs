using Lucien.HttpApi.Host;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        //builder.WebHost.ConfigureKestrel(options =>
        //{
        //    var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
        //    options.ListenAnyIP(int.Parse(port));
        //});

        ////  TEMP DIAGNOSTIC LOGS
        //Console.WriteLine("=== CONFIG CHECK START ===");
        //Console.WriteLine("DB = " + builder.Configuration.GetConnectionString("DefaultConnection"));
        //Console.WriteLine("JWT ISSUER = " + builder.Configuration["JwtSettings:Issuer"]);
        //Console.WriteLine("JWT AUDIENCE = " + builder.Configuration["JwtSettings:Audience"]);
        //Console.WriteLine("JWT SECRET EXISTS = " +
        //    (!string.IsNullOrWhiteSpace(builder.Configuration["JwtSettings:SecretKey"])));
        //Console.WriteLine("=== CONFIG CHECK END ===");

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

using Lucien.Application;
using Lucien.Application.Contracts.Settings;
using Lucien.HttpApi.Host.Logging;
using Lucien.HttpApi.Host.Middleware;
using Lucien.HttpApi.Host.Settings;
using Lucien.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Lucien.HttpApi.Host
{
    public class LucienHttpApiHostModule
    {
        public static void PreConfigureServices(IServiceCollection services, ILoggingBuilder logging, IConfiguration configuration)
        {
            string issuer = configuration["JwtSettings:Issuer"] ?? string.Empty;
            string secretKey = configuration["JwtSettings:SecretKey"] ?? string.Empty;
            string audience = configuration["JwtSettings:Audience"] ?? string.Empty;

            // Register Controllers
            ConfigureControllers(services);

            LucienApplicationModule.PreConfigureServices(services, configuration);

            LucienInfrastructureModule.PreConfigureServices(services, configuration);

            // Register CORS policy
            ConfigureCors(services, audience);

            // Register Endpoints Api Explorer
            ConfigureEndpointsApiExplorer(services);

            // Register Authentication And Pull Token from Cookies
            ConfigureAuthentication(services, issuer, audience, secretKey);

            // Register Authorization
            ConfigureAuthorization(services);

            // Register Swagger
            ConfigureSwagger(services);

            // Register Logging
            ConfigureLogging(logging, configuration);
        }

        public static void ConfigureServices(WebApplication app, IConfiguration configuration)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseCors("AllowAngularApp");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            // Configure Swagger with JWT Authentication
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your token. Example: \"your_token_here\""
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }

        private static void ConfigureLogging(ILoggingBuilder logging, IConfiguration configuration)
        {
            var loggingSettings = configuration.GetSection(LoggingSettingsConst.SectionName).Get<LoggingSettings>();

            if (string.IsNullOrEmpty(loggingSettings?.LogDirectory?.FolderName))
            {
                throw new NullReferenceException();
            }
            string logFolderName = loggingSettings.LogDirectory.FolderName;

            var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), logFolderName);

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            logging.AddProvider(new FileLoggerProvider(logDirectory));
        }

        private static void ConfigureCors(IServiceCollection services, string audience)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins(audience)
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
        }

        private static void ConfigureAuthentication(IServiceCollection services, string issuer, string audience, string secretKey)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = issuer,
                            ValidAudience = audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                        };

                        // Pull token from HttpOnly Cookies
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                if (context.Request.Cookies.TryGetValue("accessToken", out var token))
                                {
                                    context.Token = token;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    });

        }

        private static void ConfigureControllers(IServiceCollection services)
        {
            services.AddControllers();
        }

        private static void ConfigureEndpointsApiExplorer(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
        }

        private static void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization();
        }
    }
}

using Lucien.Application;
using Lucien.Application.Contracts.Settings;
using Lucien.Domain.Shared.Authorization;
using Lucien.Domain.Shared.Settings;
using Lucien.HttpApi.Host.Authorization;
using Lucien.HttpApi.Host.Logging;
using Lucien.HttpApi.Host.Middleware;
using Lucien.HttpApi.Host.Settings;
using Lucien.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Lucien.HttpApi.Host
{
    public class LucienHttpApiHostModule
    {
        public static void PreConfigureServices(IServiceCollection services, ILoggingBuilder logging, IConfiguration configuration)
        {

            var jwtSettings = configuration.GetSection(JwtConst.JwtSettings);
            var secretKey = jwtSettings[JwtConst.SecretKey] ?? string.Empty;
            var issuer = jwtSettings[JwtConst.Issuer] ?? string.Empty;
            var audience = jwtSettings[JwtConst.Audience] ?? string.Empty;

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
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseCors(CorsConst.AllowApp);

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
                c.SwaggerDoc(SwaggerConst.Version, new OpenApiInfo { Title = SwaggerConst.OpenApiInfoTitle, Version = SwaggerConst.Version });

                c.AddSecurityDefinition(SwaggerConst.SecurityDefinition, new OpenApiSecurityScheme
                {
                    Name = SwaggerConst.SecurityDefinitionName,
                    Type = SecuritySchemeType.Http,
                    Scheme = SwaggerConst.SecurityDefinitionScheme,
                    BearerFormat = SwaggerConst.SecurityDefinitionBearerFormat,
                    In = ParameterLocation.Header,
                    Description = SwaggerConst.SecurityDefinitionDescription
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = SwaggerConst.OpenApiReferenceId }
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
                options.AddPolicy(CorsConst.AllowApp, policy =>
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
                                if (context.Request.Cookies.TryGetValue(CookieConst.AaccessToken, out var token))
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
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            services.AddAuthorization(options =>
            {
                foreach (var permission in PermissionNames.All)
                {
                    options.AddPolicy(permission, policy =>
                    {
                        policy.RequireAuthenticatedUser();
                        policy.AddRequirements(new PermissionRequirement(permission));
                    });
                }
            });
        }
    }
}

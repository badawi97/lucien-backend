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
            LucienApplicationModule.PreConfigureServices(services, configuration);
            LucienInfrastructureModule.PreConfigureServices(services, configuration);

            // Register CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            options.TokenValidationParameters = new TokenValidationParameters
            //            {
            //                ValidateIssuer = true,
            //                ValidateAudience = true,
            //                ValidateLifetime = true,
            //                ValidateIssuerSigningKey = true,
            //                RequireExpirationTime = true,
            //                ValidIssuer = configuration["JwtSettings:Issuer"],
            //                ValidAudience = configuration["JwtSettings:Audience"],
            //                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]?.ToString() ?? ""))
            //            };
            //        });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        var jwtSettings = configuration["JwtSettings:Issuer"];

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration["JwtSettings:Issuer"],
                            ValidAudience = configuration["JwtSettings:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]?.ToString() ?? ""))
                        };

                        // Pull token from HttpOnly Cookie
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

            services.AddAuthorization();

            ConfigureSwagger(services);

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
    }
}

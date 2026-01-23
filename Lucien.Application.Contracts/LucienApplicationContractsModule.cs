using Lucien.Application.Contracts.Cards.Validators;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lucien.Domain.Shared;
using FluentValidation;

namespace Lucien.Application.Contracts
{
    public class LucienApplicationContractsModule
    {
        public static void PreConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            LucienDmainSharedModule.PreConfigureServices(services, configuration);

            // These must be called on IServiceCollection.
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            // Register validators (assembly scanning)
            // Todo: move AddValidatorsFromAssemblyContaining as config on entity level .
            services.AddValidatorsFromAssemblyContaining<CreateCardDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateCardDtoValidator>();
        }
    }
}

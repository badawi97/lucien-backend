using Lucien.Application.Contracts.Cards.Validators;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Lucien.Domain.Shared;

namespace Lucien.Application.Contracts
{
    public class LucienApplicationContractsModule
    {
        public static void PreConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            LucienDmainSharedModule.PreConfigureServices(services, configuration);

            services.AddControllers().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<CreateCardDtoValidator>();
            });
            services.AddControllers().AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<UpdateCardDtoValidator>();
            });
        }


    }
}

using BasketService.Api.Infrastructure.Attributes;
using BasketService.Api.Infrastructure.Filters;
using BasketService.Api.Validations.FluentValidation;
using BasketService.Api.Validations.FluentValidation.Base;
using BasketService.Api.Validations.FluentValidation.gRPC;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace BasketService.Api.Configurations.Installers.ServiceInstallers;

public class ControllerServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add(typeof(FluentValidationCustomValidationAttribute));
            options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; // default nullable types throws error, this will stop that.
        }).AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        services.AddValidatorsFromAssemblyContaining<StringModelValidator>();
        services.AddValidatorsFromAssemblyContaining<BasketItemResponseValidator>();
        services.AddValidatorsFromAssemblyContaining<BasketRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CustomerBasketRequestValidator>();

        services.AddValidatorsFromAssemblyContaining<CustomerBasketValidator>();
        services.AddValidatorsFromAssemblyContaining<BasketItemValidator>();
        services.AddValidatorsFromAssemblyContaining<BasketCheckoutValidator>();

        services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });

        return Task.CompletedTask;
    }
}

using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using BasketService.Api.Infrastructure.Filters;
using BasketService.Api.Infrastructure.Attributes;
using BasketService.Api.Validations.FluentValidation.Base;
using BasketService.Api.Validations.FluentValidation.gRPC;
using BasketService.Api.Validations.FluentValidation;

namespace BasketService.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static void AddControllerSettings(this IServiceCollection services)
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
        }
    }
}

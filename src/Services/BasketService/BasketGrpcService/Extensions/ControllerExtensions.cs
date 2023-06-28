using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using BasketGrpcService.Infrastructure.Filters;
using BasketGrpcService.Infrastructure.Attributes;
using BasketGrpcService.Validations.FluentValidation.Base;
using BasketGrpcService.Validations.FluentValidation.gRPC;
using BasketGrpcService.Validations.FluentValidation;

namespace BasketGrpcService.Extensions
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

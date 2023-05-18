using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace LocalizationService.Api.Extensions
{
    public static class ControllerExtensions
    {
        public static void AddControllerSettings(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                //options.Filters.Add(typeof(FluentValidationCustomValidationAttribute));
            }).AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //services.AddValidatorsFromAssemblyContaining<UserLoginModelValidator>();

            //services.AddFluentValidationAutoValidation();
            //services.AddFluentValidationClientsideAdapters(); // for client side
        }
    }
}

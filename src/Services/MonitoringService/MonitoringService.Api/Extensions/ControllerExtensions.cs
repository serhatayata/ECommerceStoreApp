using Microsoft.AspNetCore.Mvc;
using MonitoringService.Api.Infrastructure.Filters;
using System.Text.Json.Serialization;

namespace MonitoringService.Api.Extensions;

public static class ControllerExtensions
{
    public static void AddControllerSettings(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            //options.Filters.Add(typeof(HttpGlobalExceptionFilter));

            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        }).AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }

}

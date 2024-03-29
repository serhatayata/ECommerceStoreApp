using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace OrderService.Api.Configurations.Installers.ServiceInstallers;

public class ControllerServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
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

        return Task.CompletedTask;
    }
}

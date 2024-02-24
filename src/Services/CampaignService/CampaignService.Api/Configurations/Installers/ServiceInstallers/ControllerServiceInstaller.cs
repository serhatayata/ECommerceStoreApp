using CampaignService.Api.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 2)]
public class ControllerServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddControllers(options =>
        {
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        })
        .AddJsonOptions(o =>
        {
            o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }
}

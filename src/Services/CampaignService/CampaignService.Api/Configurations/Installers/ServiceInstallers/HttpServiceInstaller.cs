﻿using CampaignService.Api.Attributes;

namespace CampaignService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 10)]
public class HttpServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return Task.CompletedTask;
    }
}

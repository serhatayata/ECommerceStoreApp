﻿using IdentityServer.Api.Attributes;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 4)]
public class HttpServiceInstaller : IServiceInstaller
{
    public void Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}

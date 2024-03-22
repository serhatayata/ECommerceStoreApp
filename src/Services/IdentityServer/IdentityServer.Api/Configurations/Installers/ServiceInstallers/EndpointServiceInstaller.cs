﻿using IdentityServer.Api.Attributes;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 15)]
public class EndpointServiceInstaller : IServiceInstaller
{
    public void Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        services.AddEndpointsApiExplorer();
    }
}

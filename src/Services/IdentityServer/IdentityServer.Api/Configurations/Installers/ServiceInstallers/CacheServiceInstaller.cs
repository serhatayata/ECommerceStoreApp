﻿using IdentityServer.Api.Attributes;

namespace IdentityServer.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 2)]
public class CacheServiceInstaller : IServiceInstaller
{
    public Task Install(
        IServiceCollection services, 
        IConfiguration configuration, 
        IWebHostEnvironment hostEnvironment)
    {
        services.AddMemoryCache();

        return Task.CompletedTask;
    }
}

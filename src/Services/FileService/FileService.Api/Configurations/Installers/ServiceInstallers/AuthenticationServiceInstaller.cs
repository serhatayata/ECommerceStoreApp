﻿using FileService.Api.Attributes;

namespace FileService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 10)]
public class AuthenticationServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        return Task.CompletedTask;
    }
}

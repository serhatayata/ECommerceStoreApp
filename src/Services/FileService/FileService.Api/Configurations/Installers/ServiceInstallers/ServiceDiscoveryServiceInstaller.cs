﻿using Consul;
using FileService.Api.Attributes;

namespace FileService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 9)]
public class ServiceDiscoveryServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration["ConsulConfig:Address"];
            consulConfig.Address = new Uri(address);
        }));
    }
}

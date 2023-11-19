﻿using Consul;

namespace StockService.Api.Configurations.Installers.ServiceInstallers;

public class ServiceDiscoveryServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration["ServiceDiscoveryConfig:Address"];
            consulConfig.Address = new Uri(address);
        }));
    }
}

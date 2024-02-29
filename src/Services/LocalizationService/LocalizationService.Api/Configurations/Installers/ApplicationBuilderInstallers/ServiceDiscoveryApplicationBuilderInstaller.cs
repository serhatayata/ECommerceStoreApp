﻿
using Consul;
using LocalizationService.Api.Attributes;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace LocalizationService.Api.Configurations.Installers.ApplicationBuilderInstallers;

[InstallerOrder(Order = 1)]
public class ServiceDiscoveryApplicationBuilderInstaller : IApplicationBuilderInstaller
{
    public void Install(IApplicationBuilder app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
        var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
        var server = app.ApplicationServices.GetRequiredService<IServer>();

        var addressFeature = server.Features.Get<IServerAddressesFeature>();
        var addresses = addressFeature.Addresses;
        var address = addresses.First();
        Uri currentUri = new Uri(address, UriKind.Absolute);

        var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

        var uri = configuration.GetValue<Uri>("ConsulConfig:ServiceAddress");
        var serviceName = configuration.GetValue<string>("ConsulConfig:ServiceName");
        var serviceId = configuration.GetValue<string>("ConsulConfig:ServiceId");

        var registration = new AgentServiceRegistration()
        {
            ID = serviceId ?? "LocalizationService",
            Name = serviceName ?? "LocalizationService",
            Address = currentUri.Host,
            Port = currentUri.Port,
            Tags = new[] { serviceName, serviceId }
        };

        logger.LogInformation("Registering with consul");
        consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        consulClient.Agent.ServiceRegister(registration).Wait();

        //When application stops, this service will be deregistered.
        lifeTime.ApplicationStopping.Register(() =>
        {
            logger.LogInformation("Deregistering from Consul");
            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        });
    }
}

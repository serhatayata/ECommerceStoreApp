﻿using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using MonitoringService.Api.Models.Settings;

namespace MonitoringService.Api.Extensions;

public static class ServiceDiscoveryExtensions
{
    public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        try
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var server = app.ApplicationServices.GetRequiredService<IServer>();

            //var name = Dns.GetHostName(); // container id
            //var ip = Dns.GetHostEntry(name).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork); // container ip

            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            var addresses = addressFeature.Addresses;
            var address = addresses.FirstOrDefault();
            if (address == null)
                return app;

            Uri currentUri = new Uri(address, UriKind.Absolute);

            //var address = configuration.GetSection("ConsulConfig:HostAddress").Value;
            //Uri currentUri = new Uri(address, UriKind.Absolute);

            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            //var uri = configuration.GetValue<Uri>("ConsulConfig:ServiceAddress");
            var consulSettings = configuration.GetSection("ConsulConfig").Get<ConsulSettings>();

            var registration = new AgentServiceRegistration()
            {
                ID = consulSettings?.ServiceId ?? "MonitoringService",
                Name = consulSettings?.ServiceName ?? "MonitoringService",
                Address = currentUri.Host,
                Port = currentUri.Port,
                Tags = new[] { consulSettings?.ServiceName, consulSettings?.ServiceId }
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

            return app;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

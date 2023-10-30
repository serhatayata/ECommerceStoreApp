using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using NotificationService.Api.Models.Settings;

namespace NotificationService.Api.Configurations.Installers.WebApplicationInstallers;

public class ServiceDiscoveryWebAppInstaller : IWebAppInstaller
{
    public void Install(IApplicationBuilder app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        try
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var server = app.ApplicationServices.GetRequiredService<IServer>();

            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            var addresses = addressFeature.Addresses;
            var address = addresses.FirstOrDefault();
            if (address == null)
                return;

            Uri currentUri = new Uri(address, UriKind.Absolute);
            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            var consulSettings = configuration.GetSection("ConsulConfig").Get<ConsulSettings>();

            var registration = new AgentServiceRegistration()
            {
                ID = consulSettings?.ServiceId ?? "NotificationService",
                Name = consulSettings?.ServiceName ?? "NotificationService",
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
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}

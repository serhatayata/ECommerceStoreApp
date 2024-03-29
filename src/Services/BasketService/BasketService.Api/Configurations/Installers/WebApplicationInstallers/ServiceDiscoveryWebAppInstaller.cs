using BasketService.Api.Models.Settings;
using Consul;

namespace BasketService.Api.Configurations.Installers.WebApplicationInstallers;

public class ServiceDiscoveryWebAppInstaller : IWebAppInstaller
{
    public void Install(IApplicationBuilder app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        try
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var consulSettings = configuration.GetSection("ServiceDiscoveryConfig").Get<ServiceDiscoverySettings>();

            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            var registrationIds = new List<string>();
            logger.LogInformation("Registering with consul");

            //Base
            if (consulSettings != null)
            {
                if (!string.IsNullOrWhiteSpace(consulSettings.Host))
                {
                    var baseRegistrationId = RegisterConsulService(
                        app,
                        consulSettings.ServiceId,
                        consulSettings.ServiceName,
                        consulSettings.Host,
                        consulSettings.Port);

                    if (!string.IsNullOrWhiteSpace(baseRegistrationId))
                        registrationIds.Add(baseRegistrationId);
                }
            }

            //When application stops, this service will be deregistered.
            lifeTime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Deregistering from Consul");
                foreach (var registrationId in registrationIds)
                {
                    consulClient.Agent.ServiceDeregister(registrationId).Wait();
                }
            });
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private static string? RegisterConsulService(
    IApplicationBuilder app,
    string serviceId,
    string serviceName,
    string host,
    int port)
    {
        var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();

        var registration = new AgentServiceRegistration()
        {
            ID = serviceId,
            Name = serviceName,
            Address = host,
            Port = port,
            Tags = new[] { serviceName, serviceId }
        };

        consulClient.Agent.ServiceDeregister(registration.ID).Wait();
        consulClient.Agent.ServiceRegister(registration).Wait();

        return registration.ID;
    }
}

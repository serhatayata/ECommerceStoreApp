using CatalogService.Api.Models.Settings;
using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace CatalogService.Api.Extensions;

public static class ServiceDiscoveryExtensions
{
    public static IServiceCollection ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
        {
            var address = configuration["ConsulConfig:Base:Address"];
            consulConfig.Address = new Uri(address);
        }));

        return services;
    }

    public static IApplicationBuilder RegisterWithConsul(this IApplicationBuilder app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        try
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var loggingFactory = app.ApplicationServices.GetRequiredService<ILoggerFactory>();
            var server = app.ApplicationServices.GetRequiredService<IServer>();

            var addressFeature = server.Features.Get<IServerAddressesFeature>();
            var addresses = addressFeature?.Addresses;

            if (addresses == null)
                return app;



            var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

            var consulSettings = configuration.GetSection("ConsulConfig:Base").Get<ConsulSettings>();
            var consulGrpcSettings = configuration.GetSection("ConsulConfig:Grpc").Get<ConsulSettings>();

            var registrationIds = new List<string>();
            logger.LogInformation("Registering with consul");

            //Base
            if (consulSettings != null)
            {
                var address = addresses.Take(1).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(address))
                {
                    Uri currentUri = new Uri(address, UriKind.Absolute);

                    var baseRegistrationId = RegisterConsulService(
                        app,
                        consulSettings.ServiceId,
                        consulSettings.ServiceName,
                        currentUri.Host,
                        currentUri.Port);

                    if (!string.IsNullOrWhiteSpace(baseRegistrationId))
                        registrationIds.Add(baseRegistrationId);
                }
            }
            //GRPC
            if (consulGrpcSettings != null)
            {
                var address = addresses.Skip(1).Take(1).FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(address))
                {
                    Uri currentUri = new Uri(address, UriKind.Absolute);

                    var baseRegistrationId = RegisterConsulService(
                        app,
                        consulGrpcSettings.ServiceId,
                        consulGrpcSettings.ServiceName,
                        currentUri.Host,
                        currentUri.Port);

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

            return app;
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

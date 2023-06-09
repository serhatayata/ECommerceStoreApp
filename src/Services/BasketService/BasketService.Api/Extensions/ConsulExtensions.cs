﻿using Consul;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Net;
using System.Net.Sockets;

namespace BasketService.Api.Extensions
{
    public static class ConsulExtensions
    {
        public static IServiceCollection ConfigureConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                var address = configuration["ConsulConfig:Address"];
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

                //var name = Dns.GetHostName(); // container id
                //var ip = Dns.GetHostEntry(name).AddressList.FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork); // container ip

                //var addressFeature = server.Features.Get<IServerAddressesFeature>();
                //var addresses = addressFeature.Addresses;
                //var addressess = addressFeature.PreferHostingUrls;
                //var address = addresses.First();
                //Uri currentUri = new Uri(address, UriKind.Absolute);

                var address = configuration.GetSection("ConsulConfig:HostAddress").Value;
                Uri currentUri = new Uri(address, UriKind.Absolute);

                var logger = loggingFactory.CreateLogger<IApplicationBuilder>();

                var uri = configuration.GetValue<Uri>("ConsulConfig:ServiceAddress");
                var serviceName = configuration.GetValue<string>("ConsulConfig:ServiceName");
                var serviceId = configuration.GetValue<string>("ConsulConfig:ServiceId");

                var registration = new AgentServiceRegistration()
                {
                    ID = serviceId ?? "BasketService",
                    Name = serviceName ?? "BasketService",
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

                return app;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

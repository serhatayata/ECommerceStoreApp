using MassTransit;
using Microsoft.Extensions.Options;
using Shared.Queue.Events;
using StockService.Api.Extensions;
using StockService.Api.Models.Settings;

namespace StockService.Api.Configurations.Installers.ServiceInstallers;

public class MessageBrokerServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var sp = services.BuildServiceProvider();
        var queueSettings = sp.GetRequiredService<IOptions<QueueSettings>>().Value;
        var envName = hostEnvironment.EnvironmentName;

        services.AddMassTransit(m =>
        {
            //m.AddConsumer<OrderCreatedEventConsumer>();

            m.UsingRabbitMq((context, cfg) =>
            {
                // Connection
                if (envName == "Development")
                {
                    cfg.Host(host: queueSettings.Host,
                             port: (ushort)queueSettings.Port,
                             virtualHost: queueSettings.VirtualHost,
                             c =>
                             {
                                 c.Username(queueSettings.Username);
                                 c.Password(queueSettings.Password);
                             });
                }
                else if (envName == "Production")
                {
                    cfg.Host(host: queueSettings.Host);
                }

                //Send endpoints
                var stockReservedEventQueueName = MessageBrokerExtensions.GetQueueName<StockReservedEvent>();
                EndpointConvention.Map<StockReservedEvent>(new Uri($"queue:{stockReservedEventQueueName}"));

                //Subscribe
                //OrderCreatedEventConsumer
                //var nameOrderCreatedEventConsumer = MessageBrokerExtensions.GetQueueNameWithProject<OrderCreatedEventConsumer>();
                //cfg.ReceiveEndpoint(queueName: nameOrderCreatedEventConsumer, e =>
                //{
                //    e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
                //});
            });
        });
    }
}

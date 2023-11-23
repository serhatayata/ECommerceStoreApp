using MassTransit;
using Microsoft.Extensions.Options;
using Shared.Queue.Events;
using StockService.Api.Consumers;
using StockService.Api.Extensions;
using StockService.Api.Models.Settings;

namespace StockService.Api.Configurations.Installers.ServiceInstallers;

public class MessageBrokerServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var sp = services.BuildServiceProvider();
        var envName = hostEnvironment.EnvironmentName;

        services.AddMassTransit(m =>
        {
            m.AddConsumer<OrderCreatedEventConsumer>();

            m.UsingRabbitMq((context, cfg) =>
            {
                // Connection
                cfg.Host(host: configuration.GetConnectionString("RabbitMQ"));

                //Send endpoints
                var stockReservedEventQueueName = MessageBrokerExtensions.GetQueueName<StockReservedEvent>();
                EndpointConvention.Map<StockReservedEvent>(new Uri($"queue:{stockReservedEventQueueName}"));

                //Subscribe
                //OrderCreatedEventConsumer
                var nameOrderCreatedEventConsumer = MessageBrokerExtensions.GetQueueNameWithProject<OrderCreatedEventConsumer>();
                cfg.ReceiveEndpoint(queueName: nameOrderCreatedEventConsumer, e =>
                {
                    e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
                });
            });
        });
    }
}

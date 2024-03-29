using MassTransit;
using Shared.Queue.Events;
using Shared.Queue.Messages;
using StockService.Api.Consumers;
using StockService.Api.Extensions;

namespace StockService.Api.Configurations.Installers.ServiceInstallers;

public class MessageBrokerServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var sp = services.BuildServiceProvider();
        var envName = hostEnvironment.EnvironmentName;

        services.AddMassTransit(m =>
        {
            m.AddConsumer<OrderCreatedEventConsumer>();
            m.AddConsumer<StockRollbackMessageConsumer>();

            m.UsingRabbitMq((context, cfg) =>
            {
                // Connection
                cfg.Host(host: configuration.GetConnectionString("RabbitMQ"));

                //Send endpoints
                var stockReservedEventQueueName = MessageBrokerExtensions.GetQueueName<StockReservedEvent>();
                EndpointConvention.Map<StockReservedEvent>(new Uri($"queue:{stockReservedEventQueueName}"));

                //Subscribe
                //OrderCreatedEventConsumer
                var nameOrderCreatedEventConsumer = MessageBrokerExtensions.GetQueueNameWithProject<OrderCreatedEvent>();
                cfg.ReceiveEndpoint(queueName: nameOrderCreatedEventConsumer, e =>
                {
                    e.ConfigureConsumer<OrderCreatedEventConsumer>(context);
                });
                //StockRollbackMessageConsumer
                var nameStockRollbackMessageConsumer = MessageBrokerExtensions.GetQueueName<StockRollbackMessage>();
                cfg.ReceiveEndpoint(queueName: nameStockRollbackMessageConsumer, e =>
                {
                    e.ConfigureConsumer<StockRollbackMessageConsumer>(context);
                });
            });
        });

        return Task.CompletedTask;
    }
}

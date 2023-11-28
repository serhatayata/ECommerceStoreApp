using MassTransit;
using Microsoft.Extensions.Options;
using PaymentService.Api.Consumers;
using PaymentService.Api.Extensions;
using PaymentService.Api.Models.Settings;
using Shared.Queue.Events;

namespace PaymentService.Api.Configurations.Installers.ServiceInstallers;

public class MessageBrokerServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var sp = services.BuildServiceProvider();

        services.AddMassTransit(m =>
        {
            m.AddConsumer<StockReservedRequestPaymentEventConsumer>();

            m.UsingRabbitMq((context, cfg) =>
            {
                // Connection
                cfg.Host(host: configuration.GetConnectionString("RabbitMQ"));

                //Subscribe
                //StockReservedRequestPaymentEvent
                var nameStockReservedEventPaymentConsumer = MessageBrokerExtensions.GetQueueName<StockReservedRequestPaymentEvent>();
                cfg.ReceiveEndpoint(queueName: nameStockReservedEventPaymentConsumer, e =>
                {
                    e.ConfigureConsumer<StockReservedRequestPaymentEventConsumer>(context);
                });
            });
        });
    }
}

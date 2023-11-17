using MassTransit;
using Microsoft.Extensions.Options;
using OrderService.Api.Consumers;
using OrderService.Api.Extensions;
using OrderService.Api.Models.Settings;
using Shared.Queue.Events;

namespace OrderService.Api.Configurations.Installers.ServiceInstallers;

public class MessageBrokerServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var sp = services.BuildServiceProvider();
        var queueSettings = sp.GetRequiredService<IOptions<QueueSettings>>().Value;
        var envName = hostEnvironment.EnvironmentName;

        services.AddMassTransit(m =>
        {
            m.AddConsumer<PaymentCompletedEventConsumer>();
            m.AddConsumer<PaymentFailedEventConsumer>();
            m.AddConsumer<StockNotReservedEventConsumer>();

            m.UsingRabbitMq((context, cfg) =>
            {
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

                //Subscribe
                //PaymentCompletedEventConsumer
                var namePaymentCompletedEventConsumer = MessageBrokerExtensions.GetQueueNameWithProject<PaymentCompletedEventConsumer>();
                cfg.ReceiveEndpoint(queueName: namePaymentCompletedEventConsumer, e =>
                {
                    e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
                });

                //PaymentFailedEventConsumer
                var namePaymentFailedEventConsumer = MessageBrokerExtensions.GetQueueNameWithProject<PaymentFailedEventConsumer>();
                cfg.ReceiveEndpoint(queueName: namePaymentFailedEventConsumer, e =>
                {
                    e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
                });

                //PaymentFailedEventConsumer
                var stockNotReservedEventConsumer = MessageBrokerExtensions.GetQueueNameWithProject<StockNotReservedEventConsumer>();
                cfg.ReceiveEndpoint(queueName: stockNotReservedEventConsumer, e =>
                {
                    e.ConfigureConsumer<StockNotReservedEventConsumer>(context);
                });
            });
        });
    }
}

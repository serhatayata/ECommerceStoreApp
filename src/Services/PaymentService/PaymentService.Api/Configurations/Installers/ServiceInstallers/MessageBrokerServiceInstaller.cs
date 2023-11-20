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
        var queueSettings = sp.GetRequiredService<IOptions<QueueSettings>>().Value;
        var envName = hostEnvironment.EnvironmentName;

        services.AddMassTransit(m =>
        {
            m.AddConsumer<StockReservedEventConsumer>();

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

                //Subscribe
                //StockReservedEventConsumer
                var nameStockReservedEventConsumer = MessageBrokerExtensions.GetQueueName<StockReservedEvent>();
                cfg.ReceiveEndpoint(queueName: nameStockReservedEventConsumer, e =>
                {
                    e.ConfigureConsumer<StockReservedEventConsumer>(context);
                });
            });
        });
    }
}

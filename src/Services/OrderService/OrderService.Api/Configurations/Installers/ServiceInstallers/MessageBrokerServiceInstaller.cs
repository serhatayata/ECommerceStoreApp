﻿using MassTransit;
using OrderService.Api.Consumers;
using OrderService.Api.Extensions;
using Shared.Queue.Events;

namespace OrderService.Api.Configurations.Installers.ServiceInstallers;

public class MessageBrokerServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddMassTransit(m =>
        {
            m.AddConsumer<OrderCompletedRequestEventConsumer>();

            m.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host: configuration.GetConnectionString("RabbitMQ"));

                //Subscribe
                //OrderCompletedRequestEventConsumer
                var nameOrderCompletedRequestEventConsumer = MessageBrokerExtensions.GetQueueNameWithProject<OrderCompletedRequestEvent>();
                cfg.ReceiveEndpoint(queueName: nameOrderCompletedRequestEventConsumer, e =>
                {
                    e.ConfigureConsumer<OrderCompletedRequestEventConsumer>(context);
                });
            });
        });
    }
}

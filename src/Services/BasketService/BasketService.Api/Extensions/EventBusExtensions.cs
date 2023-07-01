using BasketService.Api.IntegrationEvents.EventHandling;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base;
using EventBus.Base.Abstraction;
using EventBus.Factory;
using RabbitMQ.Client;

namespace BasketService.Api.Extensions;
public static class EventBusExtensions
{
    public static void RegisterEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        var subscriptionClientName = configuration["SubscriptionClientName"];
        var retryCount = 5;
        if (!string.IsNullOrEmpty(configuration["EventBusRetryCount"]))
            retryCount = int.Parse(configuration["EventBusRetryCount"]);

        services.AddSingleton<IEventBus>(sp =>
        {
            EventBusConfig config = new()
            {
                ConnectionRetryCount = 5,
                EventNameSuffix = "IntegrationEvent",
                SubscriberClientAppName = "BasketService",
                EventBusType = EventBusType.RabbitMQ,
                Connection = new ConnectionFactory()
                {
                    HostName = "localhost"
                    //HostName = "localhost",
                    //Port = 15672,
                    //UserName = "guest",
                    //Password = "guest",
                    //VirtualHost="/"
                }

                //Connection = new ConnectionFactory()
                //{
                //    HostName = "c_rabbitmq"
                //}
            };

            return EventBusFactory.Create(config, sp);
        });

        //services.AddTransient<ProductPriceChangedIntegrationEventHandler>();
        //services.AddTransient<OrderStartedIntegrationEventHandler>(); 
    }

    public static void SubscribeEvents(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

        eventBus.Subscribe<ProductPriceChangedIntegrationEvent, ProductPriceChangedIntegrationEventHandler>();
        eventBus.Subscribe<OrderStartedIntegrationEvent, OrderStartedIntegrationEventHandler>();
    }
}
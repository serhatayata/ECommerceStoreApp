using Autofac;
using BasketGrpcService.IntegrationEvents.EventHandling;
using BasketGrpcService.Repositories.Abstract;
using BasketGrpcService.Repositories.Concrete;
using BasketGrpcService.Services.Identity.Abstract;
using BasketGrpcService.Services.Identity.Concrete;
using BasketGrpcService.Services.Localization.Abstract;
using BasketGrpcService.Services.Localization.Concrete;
using BasketGrpcService.Services.Redis.Abstract;
using BasketGrpcService.Services.Redis.Concrete;
using EventBus.Base.Abstraction;
using EventBus.Base.SubManagers;

namespace BasketGrpcService.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region Services
            builder.RegisterType<RedisService>().As<IRedisService>().SingleInstance();
            builder.RegisterType<IdentityService>().As<IIdentityService>().SingleInstance();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().SingleInstance();
            #endregion
            #region Repositories
            builder.RegisterType<BasketRepository>().As<IBasketRepository>().InstancePerLifetimeScope();
            #endregion
            #region EventBus
            builder.RegisterType<InMemoryEventBusSubscriptionManager>().As<IEventBusSubscriptionManager>().InstancePerDependency();
            #endregion
            #region EventHandlers
            builder.RegisterType<OrderStartedIntegrationEventHandler>().InstancePerDependency();
            builder.RegisterType<ProductPriceChangedIntegrationEventHandler>().InstancePerDependency();
            #endregion
        }
    }
}

using Autofac;
using BasketGrpcService.Repositories.Abstract;
using BasketGrpcService.Repositories.Concrete;
using BasketGrpcService.Services.Identity.Abstract;
using BasketGrpcService.Services.Identity.Concrete;
using BasketGrpcService.Services.Localization.Abstract;
using BasketGrpcService.Services.Localization.Concrete;
using BasketGrpcService.Services.Redis.Abstract;
using BasketGrpcService.Services.Redis.Concrete;

namespace BasketGrpcService.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RedisService>().As<IRedisService>().SingleInstance();
            builder.RegisterType<IdentityService>().As<IIdentityService>().SingleInstance();
            builder.RegisterType<BasketRepository>().As<IBasketRepository>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().SingleInstance();

        }
    }
}

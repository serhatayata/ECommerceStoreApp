using Autofac;
using BasketService.Api.Repositories.Abstract;
using BasketService.Api.Repositories.Concrete;
using BasketService.Api.Services.Identity.Abstract;
using BasketService.Api.Services.Identity.Concrete;
using BasketService.Api.Services.Localization.Abstract;
using BasketService.Api.Services.Localization.Concrete;
using BasketService.Api.Services.Redis.Abstract;
using BasketService.Api.Services.Redis.Concrete;

namespace BasketService.Api.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region Services
            builder.RegisterType<RedisService>().As<IRedisService>().SingleInstance();
            builder.RegisterType<IdentityService>().As<IIdentityService>().SingleInstance();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerLifetimeScope();
            #endregion
            #region Repositories
            builder.RegisterType<BasketRepository>().As<IBasketRepository>().InstancePerDependency();
            #endregion
        }
    }
}

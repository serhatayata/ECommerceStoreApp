using Autofac;
using BasketGrpcService.Repositories.Abstract;
using BasketGrpcService.Repositories.Concrete;
using BasketGrpcService.Services.Redis.Abstract;
using BasketGrpcService.Services.Redis.Concrete;

namespace BasketGrpcService.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RedisService>().As<IRedisService>().InstancePerLifetimeScope();
            builder.RegisterType<BasketRepository>().As<IBasketRepository>().InstancePerLifetimeScope();
        }
    }
}

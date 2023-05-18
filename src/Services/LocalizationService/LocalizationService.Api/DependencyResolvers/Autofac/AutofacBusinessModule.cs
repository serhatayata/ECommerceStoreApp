using Autofac;

namespace LocalizationService.Api.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region SERVICES
            //builder.RegisterType<RedisService>().As<IRedisService>().InstancePerLifetimeScope();
            #endregion
        }
    }
}

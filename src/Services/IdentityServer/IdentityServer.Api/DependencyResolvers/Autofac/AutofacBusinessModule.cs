using Autofac;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Services.Concrete;

namespace IdentityServer.Api.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region SERVICES
            builder.RegisterType<ClientService>().As<IClientService>().InstancePerLifetimeScope();
            #endregion
        }
    }
}

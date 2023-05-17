using Autofac;

namespace IdentityServer.Api.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region SERVICES
            //builder.RegisterType<ClientService>().As<IClientService>().InstancePerDependency();
            #endregion
        }
    }
}

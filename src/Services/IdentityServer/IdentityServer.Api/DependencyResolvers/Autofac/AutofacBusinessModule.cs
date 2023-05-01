using Autofac;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Services.Concrete;

namespace IdentityServer.Api.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region SERVICES
            builder.RegisterType<ClientService>().As<IClientService>().InstancePerDependency();
            builder.RegisterType<ApiResourceService>().As<IApiResourceService>().InstancePerDependency();
            builder.RegisterType<ApiScopeService>().As<IApiScopeService>().InstancePerDependency();
            builder.RegisterType<IdentityResourceService>().As<IIdentityResourceService>().InstancePerDependency();
            builder.RegisterType<UserService>().As<IUserService>().InstancePerDependency();
            builder.RegisterType<SharedIdentityService>().As<ISharedIdentityService>().InstancePerLifetimeScope();
            #endregion
        }
    }
}

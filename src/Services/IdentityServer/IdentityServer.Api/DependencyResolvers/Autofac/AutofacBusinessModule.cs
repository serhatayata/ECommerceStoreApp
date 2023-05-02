﻿using Autofac;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Services.Concrete;
using IdentityServer.Api.Validations.IdentityValidators;
using IdentityServer4.Services;
using IdentityServer4.Validation;

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
            builder.RegisterType<RedisCacheService>().As<IRedisCacheService>().InstancePerLifetimeScope();
            #endregion
            #region VALIDATORS
            builder.RegisterType<ResourceOwnerPasswordCustomValidator>().As<IResourceOwnerPasswordValidator>().InstancePerDependency();
            builder.RegisterType<ProfileService>().As<IProfileService>().InstancePerDependency();
            #endregion
        }
    }
}

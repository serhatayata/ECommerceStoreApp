using Autofac;
using IdentityServer.Api.CacheStores;
using IdentityServer.Api.Events.CustomEventSinks;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Services.Concrete;
using IdentityServer.Api.Services.ElasticSearch.Abstract;
using IdentityServer.Api.Services.ElasticSearch.Concrete;
using IdentityServer.Api.Services.Localization.Abstract;
using IdentityServer.Api.Services.Localization.Concrete;
using IdentityServer.Api.Services.Redis.Abstract;
using IdentityServer.Api.Services.Redis.Concrete;
using IdentityServer.Api.Utilities.Security.Jwt;
using IdentityServer.Api.Validations.IdentityValidators;
using IdentityServer4.Services;
using IdentityServer4.Stores;
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

            builder.RegisterType<RedisService>().As<IRedisService>().SingleInstance();
            builder.RegisterType<ElasticSearchService>().As<IElasticSearchService>().SingleInstance();
            builder.RegisterType<JwtHelper>().As<IJwtHelper>().SingleInstance();
            builder.RegisterType<LocalizationService>().As<ILocalizationService>().SingleInstance();
            #endregion
            #region VALIDATORS
            builder.RegisterType<ResourceOwnerPasswordCustomValidator>().As<IResourceOwnerPasswordValidator>().InstancePerDependency();
            builder.RegisterType<ProfileService>().As<IProfileService>().InstancePerDependency();
            #endregion
            #region EVENT SINKS
            builder.RegisterType<CustomEventSink>().As<IEventSink>().InstancePerDependency();
            #endregion
            #region CUSTOM STORES
            builder.RegisterType<CustomClientStore>().As<IClientStore>().SingleInstance();
            builder.RegisterType<CustomResourceStore>().As<IResourceStore>().SingleInstance();
            #endregion
        }
    }
}

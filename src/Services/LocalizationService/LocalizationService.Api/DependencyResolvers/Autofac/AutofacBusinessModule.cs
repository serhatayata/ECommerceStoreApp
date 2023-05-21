using Autofac;
using LocalizationService.Api.Data.Contexts;
using LocalizationService.Api.Data.Contexts.Connections.Abstract;
using LocalizationService.Api.Data.Contexts.Connections.Concrete;
using LocalizationService.Api.Data.Repositories.Base;
using LocalizationService.Api.Data.Repositories.Dapper.Abstract;
using LocalizationService.Api.Data.Repositories.Dapper.Concrete;
using LocalizationService.Api.Data.Repositories.EntityFramework.Abstract;
using LocalizationService.Api.Data.Repositories.EntityFramework.Concrete;
using LocalizationService.Api.Services.Abstract;
using LocalizationService.Api.Services.Concrete;
using LocalizationService.Api.Services.Redis.Abstract;
using LocalizationService.Api.Services.Redis.Concrete;

namespace LocalizationService.Api.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            #region DB
            builder.RegisterType<LocalizationReadDbConnection>().As<ILocalizationReadDbConnection>().InstancePerLifetimeScope();
            builder.RegisterType<LocalizationWriteDbConnection>().As<ILocalizationWriteDbConnection>().InstancePerLifetimeScope();

            builder.Register<ILocalizationDbContext>(l => l.Resolve<LocalizationDbContext>());
            #endregion
            #region REPOSITORIES
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();

            builder.RegisterType<DapperLanguageRepository>().As<IDapperLanguageRepository>().InstancePerDependency();
            builder.RegisterType<DapperMemberRepository>().As<IDapperMemberRepository>().InstancePerDependency();
            builder.RegisterType<DapperResourceRepository>().As<IDapperResourceRepository>().InstancePerDependency();

            builder.RegisterType<EfLanguageRepository>().As<IEfLanguageRepository>().InstancePerDependency();
            builder.RegisterType<EfMemberRepository>().As<IEfMemberRepository>().InstancePerDependency();
            builder.RegisterType<EfResourceRepository>().As<IEfResourceRepository>().InstancePerDependency();
            #endregion
            #region SERVICES
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerLifetimeScope();
            builder.RegisterType<MemberService>().As<IMemberService>().InstancePerLifetimeScope();
            builder.RegisterType<ResourceService>().As<IResourceService>().InstancePerLifetimeScope();

            builder.RegisterType<RedisService>().As<IRedisService>().InstancePerLifetimeScope();
            #endregion
        }
    }
}

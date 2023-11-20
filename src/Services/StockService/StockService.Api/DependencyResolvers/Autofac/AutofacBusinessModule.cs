using Autofac;
using StockService.Api.Repositories.Base;
using StockService.Api.Repositories.EntityFramework.Abstract;
using StockService.Api.Repositories.EntityFramework.Concrete;
using StockService.Api.Services.Abstract;

namespace StockService.Api.DependencyResolvers.Autofac;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();
        #region Repositories
        builder.RegisterType<EfStockRepository>().As<IEfStockRepository>().InstancePerDependency();
        #endregion
        #region Services
        builder.RegisterType<Services.Concrete.StockService>().As<IStockService>().InstancePerLifetimeScope();
        #endregion
    }
}

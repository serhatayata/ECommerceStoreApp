using Autofac;

namespace PaymentService.Api.DependencyResolvers.Autofac;
public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();
        //#region Repositories
        //builder.RegisterType<EfStockRepository>().As<IEfStockRepository>().InstancePerDependency();
        //#endregion
        //#region Services
        //builder.RegisterType<Services.Concrete.StockService>().As<IStockService>().InstancePerLifetimeScope();
        //#endregion
    }
}

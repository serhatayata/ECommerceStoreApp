using Autofac;
using OrderService.Api.Repositories.Base;
using OrderService.Api.Repositories.EntityFramework.Abstract;
using OrderService.Api.Repositories.EntityFramework.Concrete;
using OrderService.Api.Services.Abstract;
using Module = Autofac.Module;

namespace OrderService.Api.DependencyResolvers.Autofac;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();
        #region Repositories
        builder.RegisterType<EfOrderRepository>().As<IEfOrderRepository>().InstancePerDependency();
        #endregion
        #region Services
        builder.RegisterType<Services.Concrete.OrderService>().As<IOrderService>().InstancePerLifetimeScope();
        #endregion
    }
}

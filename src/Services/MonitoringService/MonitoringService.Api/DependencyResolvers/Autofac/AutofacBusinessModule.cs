using Autofac;
using MonitoringService.Api.Services.Abstract;
using MonitoringService.Api.Services.Concrete;

namespace MonitoringService.Api.DependencyResolvers.Autofac;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<HealthCheckDiagnosticService>().As<IHealthCheckDiagnosticService>().InstancePerDependency();
    }
}

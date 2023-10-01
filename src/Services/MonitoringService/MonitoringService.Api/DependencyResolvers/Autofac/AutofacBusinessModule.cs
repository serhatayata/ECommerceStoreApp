using Autofac;
using MonitoringService.Api.Services.HealthCheck.Abstract;
using MonitoringService.Api.Services.HealthCheck.Concrete;
using MonitoringService.Api.Services.Localization.Abstract;
using MonitoringService.Api.Services.Localization.Concrete;

namespace MonitoringService.Api.DependencyResolvers.Autofac;

public class AutofacBusinessModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<HealthCheckDiagnosticService>().As<IHealthCheckDiagnosticService>().InstancePerDependency();
        builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerDependency();
    }
}

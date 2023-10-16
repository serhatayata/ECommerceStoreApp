using Autofac;

namespace NotificationService.Api.DependencyResolvers.Autofac;

public class AutofacBusinessModel : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //builder.RegisterType<HealthCheckDiagnosticService>().As<IHealthCheckDiagnosticService>().InstancePerDependency();
        //builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerDependency();
    }
}

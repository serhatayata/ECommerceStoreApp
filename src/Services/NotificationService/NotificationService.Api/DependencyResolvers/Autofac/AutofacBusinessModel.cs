using Autofac;
using NotificationService.Api.Services.Base.Abstract;
using NotificationService.Api.Services.Base.Concrete;

namespace NotificationService.Api.DependencyResolvers.Autofac;

public class AutofacBusinessModel : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerDependency();
    }
}

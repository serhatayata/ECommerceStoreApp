using Autofac;
using NotificationService.Api.Services.Base.Abstract;
using NotificationService.Api.Services.Base.Concrete;

namespace NotificationService.Api.DependencyResolvers.Autofac;

public class AutofacBusinessModel : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        //Services
        builder.RegisterType<LocalizationService>().As<ILocalizationService>().InstancePerDependency();

        builder.RegisterType<EmailService>().As<IEmailService>().InstancePerLifetimeScope();
        builder.RegisterType<SmsService>().As<ISmsService>().InstancePerLifetimeScope();
    }
}

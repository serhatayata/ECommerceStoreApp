using MassTransit;
using Microsoft.Extensions.Options;
using OrderService.Api.Models.Settings;

namespace OrderService.Api.Configurations.Installers.ServiceInstallers;

public class MessageBrokerServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var sp = services.BuildServiceProvider();
        var queueSettings = sp.GetRequiredService<IOptions<QueueSettings>>().Value;
        var envName = hostEnvironment.EnvironmentName;

        services.AddMassTransit(m =>
        {
            m.UsingRabbitMq((context, cfg) =>
            {
                if (envName == "Development")
                {
                    cfg.Host(host: queueSettings.Host,
                             port: (ushort)queueSettings.Port,
                             virtualHost: queueSettings.VirtualHost,
                             c =>
                             {
                                 c.Username(queueSettings.Username);
                                 c.Password(queueSettings.Password);
                             });
                }
                else if (envName == "Production")
                {
                    cfg.Host(host: queueSettings.Host);
                }
            });
        });
    }
}

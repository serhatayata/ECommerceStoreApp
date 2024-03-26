
using Web.ApiGateway.Attributes;
using Web.ApiGateway.Handlers;

namespace Web.ApiGateway.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 3)]
public class HttpServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<HttpClientDelegatingHandler>();

        services.AddHttpClient("localization", c =>
        {
            c.BaseAddress = new Uri(configuration["ServiceInfo:Localization"]);
        }).AddHttpMessageHandler<HttpClientDelegatingHandler>();

        return Task.CompletedTask;
    }
}

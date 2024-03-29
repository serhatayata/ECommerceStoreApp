using IdentityServer.Api.Attributes;
using IdentityServer.Api.Configurations.Installers;

namespace CatalogService.Api.Configurations.Installers.WebApplicationInstallers;

[InstallerOrder(Order = 1)]
public class ApiDocumentationWebApplicationInstaller : IWebApplicationInstaller
{
    public void Install(WebApplication app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}

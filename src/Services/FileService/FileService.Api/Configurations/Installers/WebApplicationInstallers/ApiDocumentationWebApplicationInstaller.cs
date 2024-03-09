using FileService.Api.Attributes;

namespace FileService.Api.Configurations.Installers.WebApplicationInstallers;

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

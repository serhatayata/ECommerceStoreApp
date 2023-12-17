using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace CatalogService.Api.Configurations.Installers.WebApplicationInstallers;

public class ApiDocumentationWebApplicationInstaller : IWebApplicationInstaller
{
    public void Install(WebApplication app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            }
        });
    }
}

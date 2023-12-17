
using CatalogService.Api.Services.Grpc;

namespace CatalogService.Api.Configurations.Installers.WebApplicationInstallers;

public class GrpcWebApplicationInstaller : IWebAppInstaller
{
    public void Install(WebApplication app, IHostApplicationLifetime lifeTime, IConfiguration configuration)
    {
        app.MapGrpcHealthChecksService();

        app.MapGrpcService<GrpcBrandService>();
        app.MapGrpcService<GrpcCategoryService>();
        app.MapGrpcService<GrpcCommentService>();
        app.MapGrpcService<GrpcFeatureService>();
        app.MapGrpcService<GrpcProductService>();

        if (!app.Environment.IsProduction())
            app.MapGrpcReflectionService();
    }
}

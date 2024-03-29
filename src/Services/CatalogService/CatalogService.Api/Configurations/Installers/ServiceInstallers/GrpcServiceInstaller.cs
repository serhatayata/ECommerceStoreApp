using CatalogService.Api.Attributes;
using CatalogService.Api.Infrastructure.Interceptors;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CatalogService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 7)]
public class GrpcServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddGrpc(g =>
        {
            g.EnableDetailedErrors = true;
            g.Interceptors.Add<ExceptionInterceptor>();
        });

        services.AddGrpcHealthChecks()
                        .AddCheck("SampleHealthCheck", () => HealthCheckResult.Healthy());

        services.AddGrpcReflection();

        #region If we want to use gRPC for http1 request, we must enable AddJsonTranscoding to convert http request
        //builder.Services.AddGrpc(g =>
        //{
        //    g.EnableDetailedErrors = true;
        //    g.Interceptors.Add<ExceptionInterceptor>();
        //}).AddJsonTranscoding();
        #endregion

        return Task.CompletedTask;
    }
}
using BasketService.Api.Infrastructure.Interceptors;

namespace BasketService.Api.Configurations.Installers.ServiceInstallers;

public class GrpcServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        //services.AddGrpc(g =>
        //{
        //    g.EnableDetailedErrors = true;
        //    g.Interceptors.Add<ExceptionInterceptor>();
        //});

        services.AddGrpcReflection();

        #region If we want to use gRPC for http1 request, we must enable AddJsonTranscoding to convert http request
        services.AddGrpc(g =>
        {
            g.EnableDetailedErrors = true;
            g.Interceptors.Add<ExceptionInterceptor>();
        }).AddJsonTranscoding();
        #endregion

        return Task.CompletedTask;
    }
}

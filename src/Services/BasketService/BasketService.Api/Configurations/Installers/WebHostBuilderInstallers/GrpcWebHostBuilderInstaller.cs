using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace BasketService.Api.Configurations.Installers.WebHostBuilderInstallers;

public class GrpcWebHostBuilderInstaller : IWebHostBuilderInstaller
{
    public void Install(ConfigureWebHostBuilder builder, IWebHostEnvironment hostEnv, IConfiguration configuration)
    {
        builder.UseKestrel(options =>
        {
            var ports = GetDefinedPorts(configuration);
            options.ListenAnyIP(ports.grpcPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
            options.ListenAnyIP(ports.httpPort, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1;
            });
        });
    }

    static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
    {
        var grpcPort = config.GetValue("GRPC_PORT", 7005);
        var port = config.GetValue("PORT", 6005);
        return (port, grpcPort);
    }
}

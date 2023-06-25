using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace BasketGrpcService.Extensions
{
    public static class GrpcConfigurationExtension
    {
        public static void ConfigureGrpc(this WebApplicationBuilder builder)
        {
            builder.WebHost.UseKestrel(options => {
                var ports = GetDefinedPorts(builder.Configuration);
                options.Listen(IPAddress.Any, ports.httpPort, listenOptions => {
                    listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
                });

                options.Listen(IPAddress.Any, ports.grpcPort, listenOptions => {
                    listenOptions.Protocols = HttpProtocols.Http2;
                });
            });
        }

        static (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
        {
            var grpcPort = config.GetValue("GRPC_PORT", 7005);
            var port = config.GetValue("PORT", 80);
            return (port, grpcPort);
        }
    }
}

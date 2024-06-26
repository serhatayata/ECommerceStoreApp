﻿
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace CatalogService.Api.Configurations.Installers.WebHostBuilderInstallers;

public class GrpcWebHostBuilderInstaller : IWebHostBuilderInstaller
{
    public Task Install(ConfigureWebHostBuilder builder, IWebHostEnvironment hostEnv, IConfiguration configuration)
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

        return Task.CompletedTask;
    }

    (int httpPort, int grpcPort) GetDefinedPorts(IConfiguration config)
    {
        var grpcPort = config.GetValue("GRPC_PORT", 7006);
        var port = config.GetValue("PORT", 5006);
        return (port, grpcPort);
    }
}

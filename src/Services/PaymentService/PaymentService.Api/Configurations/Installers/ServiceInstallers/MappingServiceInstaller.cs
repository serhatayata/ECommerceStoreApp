﻿using PaymentService.Api.Mapping;

namespace PaymentService.Api.Configurations.Installers.ServiceInstallers;

public class MappingServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAutoMapper(typeof(MapProfile).Assembly);

        return Task.CompletedTask;
    }
}

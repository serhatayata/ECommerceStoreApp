﻿namespace Web.ApiGateway.Configurations.Installers;

public interface IWebApplicationInstaller
{
    void Install(WebApplication app, IHostApplicationLifetime lifeTime, IConfiguration configuration);
}

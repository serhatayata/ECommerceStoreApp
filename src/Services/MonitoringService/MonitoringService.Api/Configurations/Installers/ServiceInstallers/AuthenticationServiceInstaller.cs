﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MonitoringService.Api.Attributes;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 10)]
public class AuthenticationServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var identityServerUrl = configuration.GetSection("IdentityServerConfigurations:Url").Value;
        var identityServerAudience = configuration.GetSection("IdentityServerConfigurations:Audience").Value;

        services.AddAuthentication()
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.Authority = identityServerUrl; // IdentityServer
                options.Audience = identityServerAudience; // IdentityServer
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return Task.CompletedTask;
    }
}

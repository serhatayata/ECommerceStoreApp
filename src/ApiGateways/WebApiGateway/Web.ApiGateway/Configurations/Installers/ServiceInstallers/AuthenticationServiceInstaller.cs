using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using Web.ApiGateway.Attributes;

namespace Web.ApiGateway.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 5)]
public class AuthenticationServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var defaultScheme = "GatewayAuthenticationScheme";
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = defaultScheme;
            opt.DefaultChallengeScheme = defaultScheme;
        })
        .AddJwtBearer(defaultScheme, options =>
        {
            options.Authority = configuration.GetValue<string>("AuthConfigurations:Url");
            options.Audience = "resource_gateway";
            options.RequireHttpsMetadata = false;
            //options.RequireHttpsMetadata = false;
            //options.MetadataAddress = configuration.GetValue<string>("AuthConfigurations:UrlMetadata");
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false
            };
        });
        
        services.AddOcelot().AddConsul();

        IdentityModelEventSource.ShowPII = true;

        return Task.CompletedTask;
    }
}

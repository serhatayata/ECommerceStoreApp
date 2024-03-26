using LocalizationService.Api.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LocalizationService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 2)]
public class AuthenticationServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        var identityServerUrl = configuration.GetSection("IdentityServerConfigurations:Url").Value;
        var identityServerAudience = configuration.GetSection("IdentityServerConfigurations:Audience").Value;

        var staticIssuer = configuration.GetSection("StaticConfigurations:Issuer").Value;
        var staticAudience = configuration.GetSection("StaticConfigurations:Audience").Value;
        var staticScheme = configuration.GetSection("StaticConfigurations:Scheme").Value;
        var staticSecretKey = configuration.GetSection("StaticConfigurations:SecretKey").Value;

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
            })
            .AddJwtBearer(staticScheme, options =>
            {
                var key = Encoding.UTF8.GetBytes(staticSecretKey);
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                options.TokenValidationParameters = new()
                {
                    //ValidIssuer = staticIssuer,
                    //ValidAudience = staticAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

        return Task.CompletedTask;
    }
}

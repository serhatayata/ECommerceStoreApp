using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using MonitoringService.Api.Attributes;
using System.Security.Claims;

namespace MonitoringService.Api.Configurations.Installers.ServiceInstallers;

[InstallerOrder(Order = 11)]
public class AuthorizationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("MonitoringRead", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "monitoring_readpermission");
            });

            options.AddPolicy("MonitoringWrite", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "monitoring_writepermission");
            });

            options.AddPolicy("MonitoringReadOrWrite", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(context => context.User.HasClaim(c =>
                    (c.Type == "scope" && (c.Value == "monitoring_writepermission" || c.Value == "monitoring_readpermission"))
                ));
            });

            options.AddPolicy("MonitoringReadUser", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(context => context.User.HasClaim(c =>
                    (
                      ((c.Type == "role" || c.Type == ClaimTypes.Role || c.Type == "Roles") && (c.Value == "User.Admin" || c.Value == "User.Normal"))
                    )
                ));
            });
        });

        IdentityModelEventSource.ShowPII = true;
    }
}

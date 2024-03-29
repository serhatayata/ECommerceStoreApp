using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using System.Security.Claims;

namespace NotificationService.Api.Configurations.Installers.ServiceInstallers;

public class AuthorizationServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("NotificationRead", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "notification_readpermission");
            });

            options.AddPolicy("NotificationWrite", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "notification_writepermission");
            });

            options.AddPolicy("NotificationReadOrWrite", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(context => context.User.HasClaim(c =>
                    (c.Type == "scope" && (c.Value == "notification_writepermission" || c.Value == "notification_readpermission"))
                ));
            });

            options.AddPolicy("NotificationReadUser", policy =>
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

        return Task.CompletedTask;
    }
}
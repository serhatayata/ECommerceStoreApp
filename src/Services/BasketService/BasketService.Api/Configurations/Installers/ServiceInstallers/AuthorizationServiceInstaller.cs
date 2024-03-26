
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;

namespace BasketService.Api.Configurations.Installers.ServiceInstallers;

public class AuthorizationServiceInstaller : IServiceInstaller
{
    public Task Install(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment hostEnvironment)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("BasketRead", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "basket_readpermission");
            });

            options.AddPolicy("BasketWrite", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "basket_writepermission");
            });

            options.AddPolicy("BasketReadOrWrite", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(context => context.User.HasClaim(c =>
                    (c.Type == "scope" && (c.Value == "basket_writepermission" || c.Value == "basket_readpermission"))
                ));
            });
        });

        IdentityModelEventSource.ShowPII = true;

        return Task.CompletedTask;
    }
}

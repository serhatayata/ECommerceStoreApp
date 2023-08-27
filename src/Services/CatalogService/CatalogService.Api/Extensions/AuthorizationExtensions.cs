using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace CatalogService.Api.Extensions;

public static class AuthorizationExtensions
{
    public static void AddAuthorizationConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CatalogRead", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "catalog_readpermission");
            });

            options.AddPolicy("CatalogWrite", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("scope", "catalog_writepermission");
            });

            options.AddPolicy("CatalogReadOrWrite", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.RequireAssertion(context => context.User.HasClaim(c =>
                    (c.Type == "scope" && (c.Value == "catalog_writepermission" || c.Value == "catalog_readpermission"))
                ));
            });

            options.AddPolicy("CatalogReadUser", policy =>
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
    }
}

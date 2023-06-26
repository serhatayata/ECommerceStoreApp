using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BasketGrpcService.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void AddAuthorizationConfigurations(this IServiceCollection services, IConfiguration configuration)
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
        }
    }
}

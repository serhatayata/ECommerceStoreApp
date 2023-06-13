using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace LocalizationService.Api.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void AddAuthorizationConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var staticScheme = configuration.GetSection("StaticConfigurations:Scheme").Value;

            services.AddAuthorization(options =>
            {
                options.AddPolicy("LocalizationRead", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope","localization_readpermission");
                });

                options.AddPolicy("LocalizationWrite", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope","localization_writepermission");
                });

                options.AddPolicy("LocalizationReadOrWrite", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(context => context.User.HasClaim(c =>
                        (c.Type == "scope" && (c.Value == "localization_writepermission" || c.Value == "localization_readpermission"))
                    ));
                });

                options.AddPolicy("LocalizationStaticRead", policy =>
                {
                    //policy.RequireAuthenticatedUser();
                    policy.AddAuthenticationSchemes(staticScheme);
                    policy.RequireClaim("scope", "localization_readpermission");
                });
            });
        }
    }
}

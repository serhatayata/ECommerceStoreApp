using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace LocalizationService.Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddAuthenticationConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var identityServerUrl = configuration.GetSection("IdentityServerConfigurations:Url").Value;
            var identityServerAudience = configuration.GetSection("IdentityServerConfigurations:Audience").Value;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        }
    }
}

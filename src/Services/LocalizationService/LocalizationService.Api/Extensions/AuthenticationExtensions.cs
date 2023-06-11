using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LocalizationService.Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddAuthenticationConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            var identityServerUrl = configuration.GetSection("IdentityServerConfigurations:Url").Value;
            var identityServerAudience = configuration.GetSection("IdentityServerConfigurations:Audience").Value;

            var identityServerStaticIssuer = configuration.GetSection("IdentityServerStaticConfigurations:Issuer").Value;
            var identityServerStaticAudience = configuration.GetSection("IdentityServerStaticConfigurations:Audience").Value;
            var identityServerStaticScheme = configuration.GetSection("IdentityServerStaticConfigurations:Scheme").Value;
            var identityServerStaticSecretKey = configuration.GetSection("IdentityServerStaticConfigurations:SecretKey").Value;

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
                })
                .AddJwtBearer(identityServerStaticScheme, options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidIssuer = identityServerStaticIssuer,
                        ValidAudience = identityServerStaticAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identityServerStaticSecretKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true
                    };
                });
        }
    }
}

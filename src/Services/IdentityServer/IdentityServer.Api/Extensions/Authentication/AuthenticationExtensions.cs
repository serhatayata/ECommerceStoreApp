using IdentityServer.Api.Handlers;

namespace IdentityServer.Api.Extensions.Authentication
{
    public static class AuthenticationExtensions
    {
        public static void UseVerifyCodeTokenAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = AuthenticationSchemeConstants.VerifyCode;
            })
            .AddScheme<AppAuthenticationSchemeOptions, VerifyCodeAuthenticationHandler>(
                AuthenticationSchemeConstants.VerifyCode, options =>
                {

                }
            );
        }
    }
}

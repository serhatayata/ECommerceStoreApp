using IdentityServer.Api.Middlewares;
using Microsoft.Extensions.Options;

namespace IdentityServer.Api.Extensions.MiddlewareExtensions
{
    public static class LocalizationMiddlewareExtensions
    {
        public static IApplicationBuilder UseLocalizationMiddleware(this IApplicationBuilder builder)
        {
            var localizeOptions = builder.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            builder.UseRequestLocalization(localizeOptions.Value);
            return builder.UseMiddleware<LocalizationMiddleware>();
        }
    }
}

using IdentityServer.Api.Middlewares;

namespace MonitoringService.Api.Extensions.MiddlewareExtensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}

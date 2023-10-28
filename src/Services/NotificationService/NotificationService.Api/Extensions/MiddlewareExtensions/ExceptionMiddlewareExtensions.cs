using NotificationService.Api.Middlewares;

namespace NotificationService.Api.Extensions.MiddlewareExtensions;

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}

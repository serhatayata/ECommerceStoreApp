using CatalogService.Api.Middlewares;

namespace CatalogService.Api.Extensions.Middlewares
{
    public static class ResponseTimeMiddlewareExtensions
    {
        public static IApplicationBuilder UseResponseTimeMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseTimeMiddleware>();
        }
    }
}

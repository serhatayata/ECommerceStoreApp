﻿using IdentityServer.Api.Middlewares;

namespace IdentityServer.Api.Extensions.MiddlewareExtensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}

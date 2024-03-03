using Microsoft.AspNetCore.Http;
using MonitoringService.Api.Infrastructure.Converters;
using MonitoringService.Api.Utilities.Results;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading;

namespace IdentityServer.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next, 
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            CancellationToken cancellationToken = httpContext?.RequestAborted ?? CancellationToken.None;

            try
            {
                await _next(httpContext);
            }
            catch (UnauthorizedAccessException e)
            {
                await HandleExceptionAsync(httpContext, e, (int)HttpStatusCode.Unauthorized);
            }
            catch (BadHttpRequestException e)
            {
                await HandleExceptionAsync(httpContext, e, (int)HttpStatusCode.BadRequest);
            }
            catch (OperationCanceledException e) when (cancellationToken.IsCancellationRequested)
            {
                await HandleExceptionAsync(null, e);
            }
            catch (Grpc.Core.RpcException e) when (cancellationToken.IsCancellationRequested)
            {
                await HandleExceptionAsync(null, e);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext httpContext, 
            Exception ex,
            int statusCode = (int)HttpStatusCode.InternalServerError)
        {
            if (httpContext == null)
                return;

            CancellationToken cancellationToken = httpContext.RequestAborted;

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            _logger.LogError(new EventId(ex.HResult), ex, ex.Message);

            await httpContext.Response.WriteAsync(this.GetErrorResult(httpContext.Response.StatusCode, ex.Message).ToString(), cancellationToken);
        }


        private ErrorResultModel GetErrorResult(int statusCode, string message)
        {
            return new ErrorResultModel()
            {
                StatusCode = statusCode,
                Message = message
            };
        }
    }
}

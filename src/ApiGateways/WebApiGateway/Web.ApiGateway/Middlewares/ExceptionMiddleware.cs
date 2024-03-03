using FluentValidation;
using System.Net;
using Web.ApiGateway.Models.ErrorModels;
using Web.ApiGateway.Models.LogModels;

namespace Web.ApiGateway.Middlewares
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
            try
            {
                await _next(httpContext);
            }
            catch (UnauthorizedAccessException e)
            {
                await HandleExceptionAsync(httpContext, e, LogDetailRisks.Normal, (int)HttpStatusCode.Unauthorized);
            }
            catch (BadHttpRequestException e)
            {
                await HandleExceptionAsync(httpContext, e, LogDetailRisks.Normal, (int)HttpStatusCode.BadRequest);
            }
            catch (ValidationException e)
            {
                await HandleExceptionAsync(httpContext, e, LogDetailRisks.NotRisky, (int)HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e, LogDetailRisks.Normal);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex, LogDetailRisks risk = LogDetailRisks.Normal, int statusCode = (int)HttpStatusCode.InternalServerError)
        {
            if (httpContext == null)
                return;

            CancellationToken cancellationToken = httpContext.RequestAborted;

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            _logger.LogError(new EventId(ex.HResult), ex, ex.Message);

            await httpContext.Response.WriteAsync(this.GetErrorResult(httpContext.Response.StatusCode, ex.Message).ToString(), cancellationToken);
        }

        private ErrorDetails GetErrorResult(int statusCode, string message)
        {
            return new ErrorDetails()
            {
                StatusCode = statusCode,
                Message = message
            };
        }
    }
}

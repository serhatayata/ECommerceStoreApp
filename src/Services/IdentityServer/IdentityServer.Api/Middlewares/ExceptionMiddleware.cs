using FluentValidation;
using IdentityServer.Api.Models.ErrorModels;
using IdentityServer.Api.Models.LogModels;
using System.Net;

namespace IdentityServer.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;

        public ExceptionMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (UnauthorizedAccessException e)
            {
                await HandleExceptionAsync(httpContext, e, LogDetailRisks.Normal,(int)HttpStatusCode.Unauthorized);
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

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception e, LogDetailRisks risk = LogDetailRisks.Normal,int statusCode = (int)HttpStatusCode.InternalServerError)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            object message = string.Empty;
            string messagePrefix = "Exception :";

            //If it is because of FluentValidation
            if (e.GetType() == typeof(ValidationException))
                message = (e as ValidationException)?.Errors?.Select(s => s.ErrorMessage) ?? new List<string>();
            else
                message = e.Message;

            var logDetail = new LogDetail
            {
                MethodName = e?.TargetSite?.DeclaringType?.FullName ?? string.Empty,
                Explanation = $"{messagePrefix} : {message ?? string.Empty}",
                Risk = (byte)LogDetailRisks.Normal,
                LoggingTime = DateTime.Now.ToString()
            };

            await httpContext.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Internal Server Error"
            }.ToString());
        }
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using MonitoringService.Api.Infrastructure.Converters;
using MonitoringService.Api.Utilities.Results;
using Newtonsoft.Json;
using System.Net;

namespace MonitoringService.Api.Infrastructure.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        public HttpGlobalExceptionFilter(
            IWebHostEnvironment env, 
            ILogger<HttpGlobalExceptionFilter> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            var json = new JsonErrorResult(new[] { "An error occurred. Try it again." });

            if (env.IsDevelopment())
            {
                // If we don't serialize exception, it will fail because MethodBase cannot be serialized
                json.DeveloperMessage = JsonConvert.SerializeObject(context.Exception, Formatting.None, new DetailExceptionConverter());
                //json.DeveloperMessage = context.Exception;
            }

            context.Result = new InternalServerErrorObjectResult(json);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            context.ExceptionHandled = true;
        }
    }
}

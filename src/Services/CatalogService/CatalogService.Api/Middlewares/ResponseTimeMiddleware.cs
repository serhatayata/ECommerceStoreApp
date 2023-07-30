using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Diagnostics;

namespace CatalogService.Api.Middlewares
{
    public class ResponseTimeMiddleware
    {
        // Name of the Response Header, Custom Headers starts with "X-"  
        private const string RESPONSE_HEADER_RESPONSE_TIME = "X-Response-Time-ms";
        private long responseTimeLimit;

        // Handle to the next Middleware in the pipeline  
        private readonly RequestDelegate _next;

        public ResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(
            HttpContext context,
            IConfiguration configuration,
            ILogger<ResponseTimeMiddleware> logger)
        {
            responseTimeLimit = configuration.GetValue<long>("ResponseTimeLimit");

            ControllerActionDescriptor controllerActionDescriptor;
            var contextEndpoint = context.GetEndpoint();
            string controllerName = string.Empty;
            string actionName = string.Empty;

            if (contextEndpoint != null)
            {
                controllerActionDescriptor = contextEndpoint?.Metadata?.GetMetadata<ControllerActionDescriptor>();

                controllerName = controllerActionDescriptor?.ControllerName ?? string.Empty;
                actionName = controllerActionDescriptor?.ActionName ?? string.Empty;
            }

            // Start the Timer using Stopwatch  
            var watch = new Stopwatch();
            watch.Start();
            context.Response.OnStarting(() => {
                // Stop the timer information and calculate the time   
                watch.Stop();
                var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;

                if (responseTimeLimit < responseTimeForCompleteRequest && !string.IsNullOrWhiteSpace(controllerName) && !string.IsNullOrWhiteSpace(actionName))
                    _ = this.LogResponseTime(logger, responseTimeForCompleteRequest, controllerName, actionName);

                // Add the Response time information in the Response headers.   
                context.Response.Headers[RESPONSE_HEADER_RESPONSE_TIME] = responseTimeForCompleteRequest.ToString();
                return Task.CompletedTask;
            });
            // Call the next delegate/middleware in the pipeline   
            return this._next(context);
        }

        private async Task LogResponseTime(
            ILogger<ResponseTimeMiddleware> logger, 
            long responseTime,
            string controller,
            string action)
        {
            if (string.IsNullOrWhiteSpace(controller) || string.IsNullOrWhiteSpace(action))
                return;

            string message = "Response time limit error : {0} - Controller : {1} - Action : {2}";
            try
            {
                logger.LogError(message, responseTime, controller, action);
            }
            catch (Exception ex)
            {
                logger.LogDebug(message, responseTime);
            }
        }
    }
}

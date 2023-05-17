using FluentValidation;
using LocalizationService.Api.Models.ErrorModels;
using LocalizationService.Api.Models.LogModels;
using LocalizationService.Api.Services.ElasticSearch.Abstract;
using System.Net;

namespace LocalizationService.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IElasticSearchService _elasticSearchService;

        private ElasticSearchOptions _logOptions;

        public ExceptionMiddleware(RequestDelegate next, 
                                   IConfiguration configuration, 
                                   IElasticSearchService elasticSearchService)
        {
            _next = next;
            _configuration = configuration;
            _elasticSearchService = elasticSearchService;

            _logOptions = _configuration.GetSection("ElasticSearchOptions").Get<ElasticSearchOptions>();
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
            string messagePrefix = "Exception";
            string logIndex = _logOptions.LogIndex;

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

            try
            {
                bool clientExists = await _elasticSearchService.IndexExistsAsync(logIndex);
                if (!clientExists)
                {
                    var indexCreated = await _elasticSearchService.CreateIndexAsync<LogDetail>(logIndex);
                    if (!indexCreated)
                        throw new Exception($"{logIndex} not created");
                }

                _ = await _elasticSearchService.CreateOrUpdateAsync(logIndex, logDetail);
            }
            catch (Exception ex)
            {
                string logFileName = _configuration.GetSection("LogTextFile").Value;

                var currentDirectory = System.IO.Directory.GetCurrentDirectory();
                using StreamWriter writer = new StreamWriter($"{currentDirectory}\\{logFileName}", true);

                string textMessage = $"{messagePrefix} : {ex.Message} --- {logDetail.MethodName} --- {logDetail.Explanation} --- {logDetail.Risk} --- {logDetail.LoggingTime} \r\n";

                await writer.WriteAsync(textMessage);
            }

            await httpContext.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = "Internal Server Error"
            }.ToString());
        }
    }
}

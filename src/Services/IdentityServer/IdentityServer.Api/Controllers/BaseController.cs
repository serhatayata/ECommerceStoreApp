using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityServer.Api.Controllers
{
    public class BaseController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<BaseController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;

        public BaseController(IHttpContextAccessor httpContextAccessor, 
                              IMemoryCache memoryCache, 
                              IConfiguration configuration,
                              ILogger<BaseController> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public HtmlString Localize(string resourceKey, params object[] args)
        {
            //var acceptLanguage = _httpContextAccessor.HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value;
            //var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";

            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name ?? "tr-TR";

            //continue with memory , if not exists then send request and get response to save memory cache
            //MemoryCacheExtensions.SaveLocalizationData

            return new HtmlString(resourceKey);
        }
    }
}

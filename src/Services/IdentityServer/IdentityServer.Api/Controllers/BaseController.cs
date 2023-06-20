using IdentityServer.Api.Services.Localization.Abstract;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;

namespace IdentityServer.Api.Controllers
{
    public class BaseController : Controller
    {
        private readonly ILocalizationService _localizationService;

        public BaseController(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
        }

        public string Culture { get; set; }

        public async Task<string> Localize(string resourceKey, params object[] args)
        {
            //var acceptLanguage = _httpContextAccessor.HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value;
            //var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";

            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name ?? "tr-TR";

            var data = await _localizationService.GetStringResource(currentCulture, resourceKey, args);

            return data;
        }
    }
}

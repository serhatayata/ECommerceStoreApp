using IdentityServer.Api.Services.Localization.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var acceptLanguage = HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value;
            var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";

            this.Culture = currentCulture.Value;
        }

        public async ValueTask<string> Localize(string resourceKey, params object[] args)
        {
            var currentCulture = this.Culture;

            var data = _localizationService.GetStringResource(currentCulture, resourceKey, args);

            return data;
        }
    }
}

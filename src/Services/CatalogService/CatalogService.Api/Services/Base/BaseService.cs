using CatalogService.Api.Extensions;
using CatalogService.Api.Services.Localization.Abstract;
using Serilog;
using System.Reflection;

namespace CatalogService.Api.Services.Base
{
    public abstract class BaseService
    {
        private readonly ILocalizationService _localizationService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected BaseService(
            ILocalizationService localizationService,
            IHttpContextAccessor httpContextAccessor)
        {
            _localizationService = localizationService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string?> GetLocalizedValue(string key)
        {
            try
            {
                string culture = HttpExtensions.GetAcceptLanguage(_httpContextAccessor);
                var result = _localizationService.GetStringResource(culture, key);
                return result;
            }
            catch (Exception ex)
            {
                Log.Error("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                           ex.Message, 
                           nameof(BaseService),
                           MethodBase.GetCurrentMethod()?.Name);

                return default;
            }
        }
    }
}

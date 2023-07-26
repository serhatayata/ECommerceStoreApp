using CatalogService.Api.Extensions;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Utilities.IoC;
using System.Reflection;

namespace CatalogService.Api.Services.Grpc.Abstract
{
    public abstract class BaseGrpcFeatureService : FeatureProtoService.FeatureProtoServiceBase
    {
        private IHttpContextAccessor _httpContextAccessor;

        protected BaseGrpcFeatureService()
        {
            var httpContextAccessor = ServiceTool.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

            this.Language = this.GetAcceptLanguage(httpContextAccessor);
            this.ProjectName = System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Name ?? nameof(BaseGrpcFeatureService);
            this.ClassName = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? this.GetType().Name;
        }

        public string Language { get; set; }
        public string ProjectName { get; private set; }
        public string ClassName { get; private set; }

        private string GetAcceptLanguage(IHttpContextAccessor httpContextAccessor)
        {
            var acceptLanguage = httpContextAccessor?.HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value ?? string.Empty;
            var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";
            string culture = currentCulture;

            return culture;
        }

        public string GetCacheKey(
            string methodName,
            string[] parameters = null,
            string prefix = null)
        {
            var cacheKeyModel = new CacheKeyModel(this.ProjectName,
                                                  this.ClassName,
                                                  methodName,
                                                  prefix,
                                                  parameters);

            var cacheKey = CacheExtensions.GetCacheKeyByModel(cacheKeyModel);
            return cacheKey;
        }
    }
}

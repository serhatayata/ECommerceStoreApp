using CatalogService.Api.Extensions;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Utilities.IoC;
using System.Reflection;

namespace CatalogService.Api.Services.Grpc.Abstract
{
    public abstract class BaseGrpcBrandService : BrandProtoService.BrandProtoServiceBase
    {
        private IHttpContextAccessor _httpContextAccessor;
        
        protected BaseGrpcBrandService()
        {
            var httpContextAccessor = ServiceTool.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

            this.Language = this.GetAcceptLanguage(httpContextAccessor);
            this.ProjectName = System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Name ?? nameof(BaseGrpcBrandService);
            this.ClassName = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? this.GetType().Name;
        }

        public string Language { get; set; }
        public string ProjectName { get; private set; }
        public string ClassName { get; private set; }

        private string GetAcceptLanguage(IHttpContextAccessor httpContextAccessor)
        {
            var acceptLanguage = httpContextAccessor?.HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value;
            var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";
            string culture = currentCulture.Value;

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
                                                  this.Language,
                                                  prefix,
                                                  parameters);

            var cacheKey = CacheExtensions.GetCacheKeyByModel(cacheKeyModel);
            return cacheKey;
        }

        public string GetMethodName<T>(Type T)
        {
            return T.Name;
        }
    }
}

using CatalogService.Api.Utilities.IoC;

namespace CatalogService.Api.Services.Grpc.Abstract
{
    public abstract class BaseGrpcBrandService : BrandProtoService.BrandProtoServiceBase
    {
        private IHttpContextAccessor _httpContextAccessor;

        protected BaseGrpcBrandService()
        {
            var httpContextAccessor = ServiceTool.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

            this.Language = this.GetAcceptLanguage(httpContextAccessor);
        }

        public string Language { get; set; }


        private string GetAcceptLanguage(IHttpContextAccessor httpContextAccessor)
        {
            var acceptLanguage = httpContextAccessor?.HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value;
            var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";
            string culture = currentCulture.Value;

            return culture;
        }
    }
}

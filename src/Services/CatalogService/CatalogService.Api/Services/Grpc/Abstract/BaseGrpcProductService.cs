using CatalogService.Api.Extensions;
using System.Reflection;

namespace CatalogService.Api.Services.Grpc.Abstract;

public abstract class BaseGrpcProductService : ProductProtoService.ProductProtoServiceBase, IBaseGrpcService
{   
    private IHttpContextAccessor _httpContextAccessor;

    protected BaseGrpcProductService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        this.Language = this.GetAcceptLanguage(httpContextAccessor);
        this.ProjectName = System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Name ?? nameof(BaseGrpcProductService);
        this.ClassName = MethodBase.GetCurrentMethod()?.DeclaringType?.Name ?? this.GetType().Name;
    }

    public string Language { get; set; }
    public string ProjectName { get; private set; }
    public string ClassName { get; private set; }

    public string GetAcceptLanguage(IHttpContextAccessor httpContextAccessor)
    {
        var acceptLanguage = httpContextAccessor?.HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value ?? string.Empty;
        var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";
        string culture = currentCulture;

        return culture;
    }

    public string CurrentCacheKey(string methodName, string prefix = null, params string[] parameters)
        => CacheExtensions.GetCacheKey(methodName, this.ProjectName, this.ClassName, prefix, parameters);
}

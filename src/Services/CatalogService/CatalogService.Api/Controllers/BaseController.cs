using CatalogService.Api.Extensions;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Utilities.IoC;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CatalogService.Api.Controllers;

public abstract class BaseController<T> : ControllerBase
{
    private IHttpContextAccessor _httpContextAccessor;

    protected BaseController()
    {
        var httpContextAccessor = ServiceTool.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

        this.Language = this.GetAcceptLanguage(httpContextAccessor);
        this.ProjectName = System.Reflection.Assembly.GetEntryAssembly()?.GetName()?.Name ?? "CatalogService.Api";
        this.ClassName = typeof(T).Name;

        var configuration = ServiceTool.ServiceProvider.GetRequiredService<IConfiguration>();
        var redisOptions = ServiceTool.ServiceProvider.GetRequiredService<IOptions<RedisOptions>>().Value;

        this.DefaultCacheDuration = redisOptions.Duration;
        this.DefaultDatabaseId = redisOptions.DatabaseId;
    }

    public virtual string Language { get; set; }
    public virtual string ProjectName { get; set; }
    public virtual string ClassName { get; set; }
    public virtual int DefaultCacheDuration { get; set; }
    public virtual int DefaultDatabaseId { get; set; }

    [NonAction]
    public string GetAcceptLanguage(IHttpContextAccessor httpContextAccessor)
    {
        var acceptLanguage = httpContextAccessor?.HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value ?? string.Empty;
        var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";
        string culture = currentCulture;

        return culture;
    }

    [NonAction]
    public string CurrentCacheKey(string methodName, string prefix = null, params string[] parameters)
        => CacheExtensions.GetCacheKey(methodName, this.ProjectName, this.ClassName, prefix, parameters);

    [NonAction]
    public string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;
}

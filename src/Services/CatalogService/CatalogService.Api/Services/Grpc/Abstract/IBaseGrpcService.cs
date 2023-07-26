namespace CatalogService.Api.Services.Grpc.Abstract
{
    public interface IBaseGrpcService
    {
        string GetAcceptLanguage(IHttpContextAccessor httpContextAccessor);
        string CurrentCacheKey(string methodName, string prefix = null, params string[] parameters);
    }
}

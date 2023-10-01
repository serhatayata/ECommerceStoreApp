namespace MonitoringService.Api.Extensions;

public static class HttpExtensions
{
    public static string GetAcceptLanguage(IHttpContextAccessor httpContextAccessor)
    {
        var acceptLanguage = httpContextAccessor?.HttpContext?.Request?.GetTypedHeaders()?.AcceptLanguage?.FirstOrDefault()?.Value;
        var currentCulture = acceptLanguage.HasValue ? acceptLanguage.Value : "tr-TR";
        string culture = currentCulture.Value;

        return culture;
    }
}


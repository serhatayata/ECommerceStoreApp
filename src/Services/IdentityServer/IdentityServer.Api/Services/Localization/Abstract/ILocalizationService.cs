namespace IdentityServer.Api.Services.Localization.Abstract
{
    public interface ILocalizationService
    {
        public Task<string> GetStringResource(string culture,string resourceKey, params object[] args);
    }
}

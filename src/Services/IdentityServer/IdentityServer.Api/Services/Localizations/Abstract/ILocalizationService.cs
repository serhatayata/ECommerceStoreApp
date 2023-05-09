using IdentityServer.Api.Models.Localizations;

namespace IdentityServer.Api.Services.Localizations.Abstract
{
    public interface ILocalizationService
    {
        Task<IReadOnlyList<LocalizationResource>> GetLocalizationResources();
    }
}

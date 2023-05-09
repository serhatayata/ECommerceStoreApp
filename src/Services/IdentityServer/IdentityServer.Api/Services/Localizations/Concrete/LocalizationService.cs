using IdentityServer.Api.Models.Localizations;
using IdentityServer.Api.Services.Localizations.Abstract;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace IdentityServer.Api.Services.Localizations.Concrete
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IOptions<LocalizationConfigurations> _localizationConfigurations;

        public LocalizationService(IMemoryCache memoryCache, IOptions<LocalizationConfigurations> localizationConfigurations)
        {
            _memoryCache = memoryCache;
            _localizationConfigurations = localizationConfigurations;
        }

        public async Task<IReadOnlyList<LocalizationResource>> GetLocalizationResources()
        {
            
        }
    }
}

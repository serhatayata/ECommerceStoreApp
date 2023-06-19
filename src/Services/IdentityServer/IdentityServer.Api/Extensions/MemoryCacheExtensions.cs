using IdentityServer.Api.Dtos.Localization;
using IdentityServer.Api.Models.Settings;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityServer.Api.Extensions
{
    public static class MemoryCacheExtensions
    {
        public static void SaveLocalizationData(IMemoryCache memoryCache, IConfiguration configuration, MemberDto data)
        {
            var localizationSettings = configuration.GetSection("LocalizationSettings").Get<LocalizationSettings>();

            var localizationMemberKey = localizationSettings.MemberKey;
            var localizationMemoryDuration1 = localizationSettings.MemoryCache.Duration1;
            var localizationMemoryDuration2 = localizationSettings.MemoryCache.Duration2;

            var localizationSuffix1 = localizationSettings.MemoryCache.Suffix1;
            var localizationSuffix2 = localizationSettings.MemoryCache.Suffix2;

            var memoryCache1Prefix = $"{localizationMemberKey}-{localizationSuffix1}";
            var memoryCache2Prefix = $"{localizationMemberKey}-{localizationSuffix2}";

            //MemoryCache1
            data.Resources.ForEach(d =>
            {
                _ = memoryCache.Set($"{memoryCache1Prefix}-{d.LanguageCode}-{d.Tag}", d, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(localizationMemoryDuration1)
                });
            });

            _ = memoryCache.Set($"{memoryCache1Prefix}", "dummy1", new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(localizationMemoryDuration1)
            });

            //MemoryCache2
            data.Resources.ForEach(d =>
            {
                _ = memoryCache.Set($"{memoryCache2Prefix}-{d.LanguageCode}-{d.Tag}", d, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(localizationMemoryDuration1)
                });
            });

            _ = memoryCache.Set($"{memoryCache2Prefix}", "dummy2", new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(localizationMemoryDuration2)
            });
        }
    }
}

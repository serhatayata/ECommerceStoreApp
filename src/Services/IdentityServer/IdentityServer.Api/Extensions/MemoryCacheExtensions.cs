using IdentityServer.Api.Dtos.Localization;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityServer.Api.Extensions
{
    public static class MemoryCacheExtensions
    {
        public static void SaveLocalizationData(IMemoryCache memoryCache, IConfiguration configuration, MemberDto data)
        {
            var localizationMemberKey = configuration.GetSection("LocalizationSettings:MemberKey").Value;
            var localizationMemoryDuration1 = configuration.GetSection("LocalizationSettings:MemoryCache:Duration1").Get<int>();
            var localizationMemoryDuration2 = configuration.GetSection("LocalizationSettings:MemoryCache:Duration2").Get<int>();

            var localizationSuffix1 = configuration.GetSection("LocalizationSettings:MemoryCache:Suffix1").Value;
            var localizationSuffix2 = configuration.GetSection("LocalizationSettings:MemoryCache:Suffix2").Value;

            var memoryCache1Prefix = $"{localizationMemberKey}-{localizationSuffix1}";
            var memoryCache2Prefix = $"{localizationMemberKey}-{localizationSuffix2}";

            string dummyData1 = $"{localizationMemberKey}-{localizationSuffix1}";
            string dummyData2 = $"{localizationMemberKey}-{localizationSuffix2}";

            //MemoryCache1
            data.Resources.ForEach(d =>
            {
                _ = memoryCache.Set($"{memoryCache1Prefix}-{d.LanguageCode}-{d.Tag}", d, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(localizationMemoryDuration1),
                    Priority = CacheItemPriority.High
                });
            });

            _ = memoryCache.Set($"{memoryCache2Prefix}", "dummy1", new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(localizationMemoryDuration1),
                Priority = CacheItemPriority.High
            });

            //MemoryCache2
            data.Resources.ForEach(d =>
            {
                _ = memoryCache.Set($"{memoryCache2Prefix}-{d.LanguageCode}-{d.Tag}", d, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(localizationMemoryDuration1),
                    Priority = CacheItemPriority.High
                });
            });

            _ = memoryCache.Set($"{memoryCache2Prefix}", "dummy2", new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(localizationMemoryDuration2),
                Priority = CacheItemPriority.High
            });
        }
    }
}

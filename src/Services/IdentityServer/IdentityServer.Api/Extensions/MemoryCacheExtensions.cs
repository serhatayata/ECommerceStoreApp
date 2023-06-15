using IdentityServer.Api.Dtos.Localization;
using Microsoft.Extensions.Caching.Memory;

namespace IdentityServer.Api.Extensions
{
    public static class MemoryCacheExtensions
    {
        public static void SaveLocalizationData(IMemoryCache memoryCache, IConfiguration configuration, List<MemberDto> data)
        {
            var localizationMemberKey = configuration.GetSection("LocalizationSettings:MemberKey").Value;
            var localizationMemoryDuration1 = configuration.GetSection("LocalizationSettings:MemoryCache:Duration1").Get<int>();
            var localizationMemoryDuration2 = configuration.GetSection("LocalizationSettings:MemoryCache:Duration2").Get<int>();

            var localizationSuffix1 = configuration.GetSection("LocalizationSettings:MemoryCache:Suffix1").Value;
            var localizationSuffix2 = configuration.GetSection("LocalizationSettings:MemoryCache:Suffix2").Value;

            //MemoryCache1
            _ = memoryCache.Set($"{localizationMemberKey}-{localizationSuffix1}", data, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(localizationMemoryDuration1),
                Priority = CacheItemPriority.High
            });

            //MemoryCache2
            _ = memoryCache.Set($"{localizationMemberKey}-{localizationSuffix2}", data, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTime.Now.AddHours(localizationMemoryDuration2),
                Priority = CacheItemPriority.High
            });
        }
    }
}

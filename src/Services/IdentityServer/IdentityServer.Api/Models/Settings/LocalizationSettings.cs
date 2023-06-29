namespace IdentityServer.Api.Models.Settings
{
    public class LocalizationSettings
    {
        public string MemberKey { get; set; }
        public MemoryCacheSettings MemoryCache { get; set; }
        public int CacheDuration { get; set; }
        public string Scheme { get; set; }
        public int DatabaseId { get; set; }
    }

    public class MemoryCacheSettings
    {
        public int Duration1 { get; set; }
        public int Duration2 { get; set; }
        public string Suffix1 { get; set; }
        public string Suffix2 { get; set; }
    }
}

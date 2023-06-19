namespace IdentityServer.Api.Models.Settings
{
    public class RedisSettings
    {
        public string ConnectionString { get; set; }
        public string DbName { get; set; }
        public int LocalizationCacheDbId { get; set; }
    }
}

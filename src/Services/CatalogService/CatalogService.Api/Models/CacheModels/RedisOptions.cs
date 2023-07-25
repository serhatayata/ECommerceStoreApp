namespace CatalogService.Api.Models.CacheModels
{
    public class RedisOptions
    {
        public string ConnectionString { get; set; }
        public string DbName { get; set; }
        public int DatabaseId { get; set; }
        public string Prefix { get; set; }
        public int Duration { get; set; }
    }
}

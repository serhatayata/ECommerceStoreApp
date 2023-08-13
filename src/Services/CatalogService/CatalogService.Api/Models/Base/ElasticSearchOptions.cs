namespace CatalogService.Api.Models.Base
{
    public class ElasticSearchOptions
    {
        public string ConnectionString { get; set; }
        public string AuthUserName { get; set; }
        public string AuthPassword { get; set; }
        public string Prefix { get; set; }
    }
}

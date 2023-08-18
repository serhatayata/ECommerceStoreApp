namespace CatalogService.Api.Models.ProductModels
{
    public class ProductSuggest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SuggestedName { get; set; }
        public double Score { get; set; }
    }
}

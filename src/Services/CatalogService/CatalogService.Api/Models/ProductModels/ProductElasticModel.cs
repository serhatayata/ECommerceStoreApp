using Nest;

namespace CatalogService.Api.Models.ProductModels
{
    public class ProductElasticModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int AvailableStock { get; set; }
        public string Link { get; set; }
        public string ProductCode { get; set; }
        public int? ProductTypeId { get; set; }
        public int? BrandId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        // Elastic model

        public CompletionField NameSuggest { get; set; }
    }
}

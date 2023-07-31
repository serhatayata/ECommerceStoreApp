using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.FeatureModels
{
    public class ProductFeaturePropertyAddModel : IModel
    {
        public int ProductFeatureId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}

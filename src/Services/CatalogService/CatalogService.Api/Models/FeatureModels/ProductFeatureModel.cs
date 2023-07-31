using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.FeatureModels
{
    public class ProductFeatureModel : IModel
    {
        public int FeatureId { get; set; }
        public int ProductId { get; set; }
    }
}

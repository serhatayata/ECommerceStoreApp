using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.FeatureModels
{
    public class ProductFeatureAddModel : IModel
    {
        public int FeatureId { get; set; }
        public int ProductId { get; set; }
    }
}

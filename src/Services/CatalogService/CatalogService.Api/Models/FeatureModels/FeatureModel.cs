using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.FeatureModels
{
    public class FeatureModel : IModel
    {
        public string Name { get; set; }

        public ICollection<ProductFeature> ProductFeatures { get; set; }
    }
}

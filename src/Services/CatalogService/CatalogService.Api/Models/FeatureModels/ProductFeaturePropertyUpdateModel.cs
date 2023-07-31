using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.FeatureModels
{
    public class ProductFeaturePropertyUpdateModel : IModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

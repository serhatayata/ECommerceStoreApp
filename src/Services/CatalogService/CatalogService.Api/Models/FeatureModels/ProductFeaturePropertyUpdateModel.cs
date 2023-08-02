using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.FeatureModels
{
    public class ProductFeaturePropertyUpdateModel : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

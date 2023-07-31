using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.FeatureModels
{
    public class FeatureUpdateModel : IModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}

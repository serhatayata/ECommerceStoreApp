using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.BrandModels
{
    public class BrandUpdateModel : IModel
    {
        /// <summary>
        /// Id of the brand
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the brand
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of the brand
        /// </summary>
        public string Description { get; set; }
    }
}

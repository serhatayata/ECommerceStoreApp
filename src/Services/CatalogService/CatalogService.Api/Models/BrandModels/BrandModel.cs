using CatalogService.Api.Entities;

namespace CatalogService.Api.Models.BrandModels
{
    public class BrandModel
    {
        /// <summary>
        /// Name of the brand
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of the brand
        /// </summary>
        public string Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}

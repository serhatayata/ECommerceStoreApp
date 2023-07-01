using CatalogService.Api.Entities.Abstract;

namespace CatalogService.Api.Entities
{
    public class ProductType : IEntity
    {
        /// <summary>
        /// Id of the product
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of the product
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of the product
        /// </summary>
        public string Description { get; set; }
    }
}

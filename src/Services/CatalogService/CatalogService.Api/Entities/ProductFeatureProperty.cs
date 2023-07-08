using CatalogService.Api.Entities.Abstract;

namespace CatalogService.Api.Entities
{
    public class ProductFeatureProperty : IEntity
    {
        /// <summary>
        /// Id of the product feature property
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Product feature id
        /// </summary>
        public int ProductFeatureId { get; set; }
        /// <summary>
        /// Description of the property
        /// </summary>
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Product feature entity FK
        /// </summary>
        public ProductFeature ProductFeature { get; set; }
    }
}

using CatalogService.Api.Entities.Abstract;

namespace CatalogService.Api.Entities
{
    public class Brand : IEntity
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

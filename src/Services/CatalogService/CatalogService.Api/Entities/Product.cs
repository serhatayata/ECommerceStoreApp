using CatalogService.Api.Entities.Abstract;

namespace CatalogService.Api.Entities
{
    public class Product : IEntity
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
        /// <summary>
        /// Price of the product
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Available stock of the product
        /// </summary>
        public int AvailableStock { get; set; }
        /// <summary>
        /// Link of the product
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Product code
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Type id of the product
        /// </summary>
        public int? ProductTypeId { get; set; }
        /// <summary>
        /// Brand if of the product
        /// </summary>
        public int? BrandId { get; set; }
        /// <summary>
        /// Create date of the product
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Update date of the product
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Product type entity FK
        /// </summary>
        public ProductType? ProductType { get; set; }
        /// <summary>
        /// Brand entity FK
        /// </summary>
        public Brand? Brand { get; set; }
        /// <summary>
        /// One to many product features
        /// </summary>
        public ICollection<ProductFeature> ProductFeatures { get; set; }
        /// <summary>
        /// One to many comments
        /// </summary>
        public ICollection<Comment> Comments { get; set; }
    }
}

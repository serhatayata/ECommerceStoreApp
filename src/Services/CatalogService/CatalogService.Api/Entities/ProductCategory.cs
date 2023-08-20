using CatalogService.Api.Entities.Abstract;
using System.ComponentModel.DataAnnotations;

namespace CatalogService.Api.Entities
{
    public class ProductCategory : IEntity
    {
        /// <summary>
        /// Product id
        /// </summary>
        public int ProductId { get; set; }
        /// <summary>
        /// Category id
        /// </summary>
        public int CategoryId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Category Category { get; set; }
    }
}

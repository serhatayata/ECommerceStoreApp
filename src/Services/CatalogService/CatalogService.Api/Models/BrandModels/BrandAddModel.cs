using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.BrandModels
{
    public class BrandAddModel : IModel
    {
        public BrandAddModel()
        {
            
        }

        public BrandAddModel(
            string name, 
            string description)
        {
            this.Name = name;
            this.Description = description;
        }

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

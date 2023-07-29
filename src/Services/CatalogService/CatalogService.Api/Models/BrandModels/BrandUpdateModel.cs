using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.BrandModels
{
    public class BrandUpdateModel : IModel
    {
        public BrandUpdateModel()
        {
            
        }

        public BrandUpdateModel(
            int id,
            string name,
            string description)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
        }

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

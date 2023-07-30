using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.CategoryModels
{
    public class CategoryAddModel : IModel
    {
        public CategoryAddModel()
        {
            
        }

        public CategoryAddModel(
            int? parentId,
            string name,
            int line)
        {
            this.ParentId = parentId;
            this.Name = name;
            this.Line = line;
        }

        /// <summary>
        /// ParentId of the category
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// Name of the category
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Line of the category
        /// </summary>
        public int Line { get; set; }
    }
}

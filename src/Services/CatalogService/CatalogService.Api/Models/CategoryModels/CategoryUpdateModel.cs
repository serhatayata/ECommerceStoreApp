using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.CategoryModels
{
    public class CategoryUpdateModel : IModel
    {
        public CategoryUpdateModel()
        {

        }

        public CategoryUpdateModel(
            int? parentId,
            string name,
            string link,
            int line)
        {
            this.ParentId = parentId;
            this.Name = name;
            this.Link = link;
            this.Line = line;
        }

        /// <summary>
        /// Id of the category
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// ParentId of the category
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// Name of the category
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Link of the category
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Line of the category
        /// </summary>
        public int Line { get; set; }
    }
}

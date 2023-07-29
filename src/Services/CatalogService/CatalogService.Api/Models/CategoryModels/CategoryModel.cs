using CatalogService.Api.Models.Base.Abstract;
using CatalogService.Api.Models.ProductModels;

namespace CatalogService.Api.Models.CategoryModels
{
    public class CategoryModel : IModel
    {
        public CategoryModel()
        {
            
        }

        public CategoryModel(
            int? parentId,
            string name,
            string link,
            int line,
            DateTime createDate,
            DateTime updateDate)
        {
            this.ParentId = parentId;
            this.Name = name;
            this.Link = link;
            this.Line = line;
            this.CreateDate = createDate;
            this.UpdateDate = updateDate;
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
        /// Link of the category
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Line of the category
        /// </summary>
        public int Line { get; set; }
        /// <summary>
        /// Create date of the category
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Update date of the category
        /// </summary>
        public DateTime UpdateDate { get; set; }

        public virtual CategoryModel ParentCategory { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; }
        public virtual ICollection<CategoryModel> SubCategories { get; set; }
    }
}

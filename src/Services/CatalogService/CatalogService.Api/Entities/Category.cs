using CatalogService.Api.Entities.Abstract;

namespace CatalogService.Api.Entities;

public class Category : IEntity
{
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
    /// Code of the category
    /// </summary>
    public string Code { get; set; }
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
    /// <summary>
    /// Parent Category
    /// </summary>
    public virtual Category ParentCategory { get; set; }

    public virtual ICollection<Category> SubCategories { get; set; }
    public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    public virtual ICollection<Product> Products { get; set; }
}

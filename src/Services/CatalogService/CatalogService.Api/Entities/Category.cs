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
    public int ParentId { get; set; }
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
    public DateTime UpdateTime { get; set; }

    public ICollection<Product> Products { get; set; }
}

using CatalogService.Api.Entities.Abstract;

namespace CatalogService.Api.Entities;

public class ProductFeature : IEntity
{
    /// <summary>
    /// Id of the product feature
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Id of the feature
    /// </summary>
    public int FeatureId { get; set; }
    /// <summary>
    /// Id of the product
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Feature entity FK
    /// </summary>
    public Feature Feature { get; set; }
    /// <summary>
    /// Product entity FK
    /// </summary>
    public Product Product { get; set; }
}

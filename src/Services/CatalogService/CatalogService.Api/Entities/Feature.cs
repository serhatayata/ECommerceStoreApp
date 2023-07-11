using CatalogService.Api.Entities.Abstract;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Entities;

public class Feature : IEntity
{
    /// <summary>
    /// Id of the feature
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Name of the feature
    /// </summary>
    public string Name { get; set; }

    public ICollection<ProductFeature> ProductFeatures { get; set; }
}

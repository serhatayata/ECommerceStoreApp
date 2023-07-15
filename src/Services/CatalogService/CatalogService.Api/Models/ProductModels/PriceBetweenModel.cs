using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.ProductModels;

public class PriceBetweenModel : IModel
{
    /// <summary>
    /// Minimum price of the criteria
    /// </summary>
    public decimal MinimumPrice { get; set; }
    /// <summary>
    /// Maximum price of the criteria
    /// </summary>
    public decimal MaximumPrice { get; set; }
}

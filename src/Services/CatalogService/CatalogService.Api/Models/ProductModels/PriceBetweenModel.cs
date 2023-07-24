using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.ProductModels;

public class PriceBetweenModel : IModel
{
    public PriceBetweenModel()
    {
        
    }

    public PriceBetweenModel(decimal minimumPrice, decimal maximumPrice)
    {
        this.MinimumPrice = minimumPrice;
        this.MaximumPrice = maximumPrice;
    }

    /// <summary>
    /// Minimum price of the criteria
    /// </summary>
    public decimal MinimumPrice { get; set; }
    /// <summary>
    /// Maximum price of the criteria
    /// </summary>
    public decimal MaximumPrice { get; set; }
}

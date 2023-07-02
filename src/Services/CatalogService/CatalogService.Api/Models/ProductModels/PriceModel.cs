using CatalogService.Api.Models.Base.Abstract;

namespace CatalogService.Api.Models.ProductModels;

public class PriceModel : IModel
{
    public decimal MinimumPrice { get; set; }
    public decimal MaximumPrice { get; set; }
}

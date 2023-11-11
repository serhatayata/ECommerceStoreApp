using StockService.Api.Models.Base;

namespace StockService.Api.Models.StockModels;

public class StockAddModel : IModel
{
    public int ProductId { get; set; }
    public int Count { get; set; }
}

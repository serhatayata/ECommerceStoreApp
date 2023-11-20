namespace StockService.Api.Models.StockModels;

public class StockAddModel : Base.IModel
{
    public int ProductId { get; set; }
    public int Count { get; set; }
}

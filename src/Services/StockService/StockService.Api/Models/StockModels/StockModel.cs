namespace StockService.Api.Models.StockModels;

public class StockModel : Base.IModel
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Count { get; set; }
}

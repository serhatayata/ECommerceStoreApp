namespace StockService.Api.Models.StockModels;

public class StockUpdateModel : Base.IModel
{
    public int Id { get; set; }
    public int Count { get; set; }
}

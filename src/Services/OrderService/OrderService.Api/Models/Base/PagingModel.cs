namespace OrderService.Api.Models.Base;

public class PagingModel : IModel
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 8;
}

using OrderService.Api.Entities;
using OrderService.Api.Models.Base;
using OrderService.Api.Models.Enums;

namespace OrderService.Api.Models.OrderItemModels;

public class OrderItemModel : IModel
{
    public int ProductId { get; set; }

    public decimal Price { get; set; }

    public int OrderId { get; set; }

    public int Count { get; set; }

    public Order Order { get; set; }
}

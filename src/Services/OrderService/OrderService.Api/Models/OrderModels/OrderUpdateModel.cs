using OrderService.Api.Models.Base;
using OrderService.Api.Models.Enums;
using OrderService.Api.Models.OrderItemModels;

namespace OrderService.Api.Models.OrderModels;

public class OrderUpdateModel : IModel
{
    /// <summary>
    /// Id of the order
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Id of the buyer
    /// </summary>
    public string BuyerId { get; set; }

    /// <summary>
    /// Status of the order
    /// </summary>
    public OrderStatus Status { get; set; }

    /// <summary>
    /// Message if fails
    /// </summary>
    public string? FailMessage { get; set; }

    /// <summary>
    /// Address of the order
    /// </summary>
    public AddressModel Address { get; set; }

    /// <summary>
    /// Items in this order
    /// </summary>
    public ICollection<OrderItemModel> Items { get; set; } = new List<OrderItemModel>();
}

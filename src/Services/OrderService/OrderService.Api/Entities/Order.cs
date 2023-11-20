using OrderService.Api.Models.Enums;

namespace OrderService.Api.Entities;

public class Order : IEntity
{
    /// <summary>
    /// Id of the order
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Code of the order
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Creation time of the order
    /// </summary>
    public DateTime CreatedDate { get; set; }

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
    public Address Address { get; set; }

    /// <summary>
    /// Items in this order
    /// </summary>
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}

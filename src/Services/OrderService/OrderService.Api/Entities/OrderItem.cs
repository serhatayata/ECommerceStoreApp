namespace OrderService.Api.Entities;

/// <summary>
/// Information about order's item
/// </summary>
public class OrderItem : IEntity
{
    /// <summary>
    /// Id of the item
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Id of the product
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Price of the order item
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Id of the order, which includes this order item
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Count of the order item
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Navigation property of order
    /// </summary>
    public Order Order { get; set; }
}

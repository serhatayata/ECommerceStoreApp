using Shared.Queue.Models;

namespace Shared.Queue.Events;

public class StockReservedEvent
{
    public int OrderId { get; set; }
    public string BuyerId { get; set; }
    public PaymentMessage Payment { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; } = new();
}

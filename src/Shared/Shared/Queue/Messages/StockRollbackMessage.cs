using Shared.Queue.Models;

namespace Shared.Queue.Messages;

public class StockRollbackMessage : IStockRollbackMessage
{
    public List<OrderItemMessage> OrderItems { get; set; } = new();
    public int OrderId { get; set; }
}

using Shared.Queue.Models;

namespace Shared.Queue.Messages;

public interface IStockRollbackMessage
{
    int OrderId { get; set; }
    List<OrderItemMessage> OrderItems { get;set; }
}

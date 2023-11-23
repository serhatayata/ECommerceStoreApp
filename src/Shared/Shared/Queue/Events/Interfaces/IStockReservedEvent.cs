using MassTransit;
using Shared.Queue.Models;

namespace Shared.Queue.Events.Interfaces;

public interface IStockReservedEvent : CorrelatedBy<Guid>
{
    List<OrderItemMessage> OrderItems { get; set; }
}

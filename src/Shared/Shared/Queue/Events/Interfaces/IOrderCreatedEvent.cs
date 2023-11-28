using MassTransit;
using Shared.Queue.Models;

namespace Shared.Queue.Events.Interfaces;

public interface IOrderCreatedEvent : CorrelatedBy<Guid>
{
    List<OrderItemMessage> OrderItems { get; set; }
}

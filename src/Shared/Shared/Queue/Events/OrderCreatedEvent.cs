using Shared.Queue.Events.Interfaces;
using Shared.Queue.Models;

namespace Shared.Queue.Events;

public class OrderCreatedEvent : IOrderCreatedEvent
{
    public OrderCreatedEvent(Guid correlationId)
    {
        this.CorrelationId = correlationId;
    }

    public List<OrderItemMessage> OrderItems { get; set; } = new List<OrderItemMessage>();

    public Guid CorrelationId { get; }
}

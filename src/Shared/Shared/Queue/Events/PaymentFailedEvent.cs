using Shared.Queue.Events.Interfaces;
using Shared.Queue.Models;

namespace Shared.Queue.Events;

public class PaymentFailedEvent : IPaymentFailedEvent
{
    public PaymentFailedEvent(Guid correlationId)
    {
        this.CorrelationId = correlationId;       
    }

    public List<OrderItemMessage> OrderItems { get; set; }
    public string Reason { get; set; }

    public Guid CorrelationId { get; }
}

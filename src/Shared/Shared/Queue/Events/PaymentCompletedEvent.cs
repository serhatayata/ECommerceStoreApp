using Shared.Queue.Events.Interfaces;

namespace Shared.Queue.Events;

public class PaymentCompletedEvent : IPaymentCompletedEvent
{
    public PaymentCompletedEvent(Guid correlationId)
    {
        this.CorrelationId = correlationId;
    }

    public Guid CorrelationId { get; }
}

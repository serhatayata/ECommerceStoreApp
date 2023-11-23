using Shared.Queue.Events.Interfaces;
using Shared.Queue.Models;

namespace Shared.Queue.Events;

public class StockReservedRequestPaymentEvent : IStockReservedRequestPaymentEvent
{
    public StockReservedRequestPaymentEvent(Guid correlationId)
    {
        this.CorrelationId = correlationId;
    }

    public PaymentMessage Payment { get; set; }
    public List<OrderItemMessage> OrderItems { get; set; } = new();

    public Guid CorrelationId { get; }
}

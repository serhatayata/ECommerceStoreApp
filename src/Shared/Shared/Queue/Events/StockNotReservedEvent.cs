using Shared.Queue.Events.Interfaces;
using Shared.Queue.Models;

namespace Shared.Queue.Events;

public class StockNotReservedEvent : IStockNotReservedEvent
{
    public StockNotReservedEvent(Guid correlationId)
    {
        this.CorrelationId = correlationId;
    }

    public string Reason { get; set; }

    public Guid CorrelationId { get; }
}

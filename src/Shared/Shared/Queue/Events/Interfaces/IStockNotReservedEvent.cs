using MassTransit;

namespace Shared.Queue.Events.Interfaces;

public interface IStockNotReservedEvent : CorrelatedBy<Guid>
{
    string Reason { get; set; }
}

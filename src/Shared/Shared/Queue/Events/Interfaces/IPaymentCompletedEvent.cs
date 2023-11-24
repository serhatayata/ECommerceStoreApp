using MassTransit;

namespace Shared.Queue.Events.Interfaces;

public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
{
}

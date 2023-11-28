using MassTransit;
using Shared.Queue.Models;

namespace Shared.Queue.Events.Interfaces;

public interface IPaymentFailedEvent : CorrelatedBy<Guid>
{
    string Reason { get; set; }

    /// <summary>
    /// Items for compensable transaction 
    /// </summary>
    List<OrderItemMessage> OrderItems { get; set; }
}

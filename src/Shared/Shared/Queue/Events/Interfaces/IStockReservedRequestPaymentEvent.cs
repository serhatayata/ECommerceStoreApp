using MassTransit;
using Shared.Queue.Models;

namespace Shared.Queue.Events.Interfaces;

public interface IStockReservedRequestPaymentEvent : CorrelatedBy<Guid>
{
    PaymentMessage Payment { get; set; }

    /// <summary>
    /// If payment fails, then we use this for compensable transaction (e.g. stock service)
    /// </summary>
    List<OrderItemMessage> OrderItems { get; set; }
}

using Shared.Queue.Events.Interfaces;

namespace Shared.Queue.Events;

public class OrderCompletedRequestEvent : IOrderCompletedRequestEvent
{
    public int OrderId { get; set; }
}

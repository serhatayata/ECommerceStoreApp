using Shared.Queue.Events.Interfaces;

namespace Shared.Queue.Events;

public class OrderFailedRequestEvent : IOrderFailedRequestEvent
{
    public int OrderId { get; set; }
    public string Reason { get; set; }
}

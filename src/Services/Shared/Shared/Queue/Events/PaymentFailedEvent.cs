namespace Shared.Queue.Events;

public class PaymentFailedEvent
{
    public int OrderId { get; set; }
    public string BuyerId { get; set; }
    public string Message { get; set; }
}

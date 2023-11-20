namespace Shared.Queue.Events;

public class PaymentCompletedEvent
{
    public int OrderId { get; set; }
    public string BuyerId { get; set; }
}

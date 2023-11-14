namespace Shared.Queue.Events;

public class PaymentSucceededEvent
{
    public int OrderId { get; set; }
    public string BuyerId { get; set; }
}

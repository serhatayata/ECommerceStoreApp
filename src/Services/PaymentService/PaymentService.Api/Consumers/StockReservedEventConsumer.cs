using MassTransit;
using Shared.Queue.Events;

namespace PaymentService.Api.Consumers;

public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
{
    private readonly ILogger<StockReservedEventConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public StockReservedEventConsumer(
        ILogger<StockReservedEventConsumer> logger, 
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<StockReservedEvent> context)
    {
        var balance = 3000m;

        if (balance > context.Message.Payment.TotalPrice)
        {
            _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from credit card for user id {context.Message.BuyerId}");

            await _publishEndpoint.Publish(new PaymentCompletedEvent()
            {
                OrderId = context.Message.OrderId,
                BuyerId = context.Message.BuyerId
            });
        }
        else
        {
            _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was NOT   withdrawn from credit card for user id {context.Message.BuyerId}");

            await _publishEndpoint.Publish(new PaymentFailedEvent()
            {
                OrderId = context.Message.OrderId,
                BuyerId = context.Message.BuyerId,
                Message = "Not withdrawn because of balance"
            });
        }
    }
}

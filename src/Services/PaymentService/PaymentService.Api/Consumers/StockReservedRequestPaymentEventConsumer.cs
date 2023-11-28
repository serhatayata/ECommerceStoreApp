using MassTransit;
using Shared.Queue.Events;
using Shared.Queue.Events.Interfaces;

namespace PaymentService.Api.Consumers;

public class StockReservedRequestPaymentEventConsumer : IConsumer<IStockReservedRequestPaymentEvent>
{
    private readonly ILogger<StockReservedRequestPaymentEventConsumer> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public StockReservedRequestPaymentEventConsumer(
        ILogger<StockReservedRequestPaymentEventConsumer> logger,
        IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<IStockReservedRequestPaymentEvent> context)
    {
        var balance = 3000m;

        if (balance > context.Message.Payment.TotalPrice)
        {
            _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was withdrawn from credit card for buyer id {context.Message.BuyerId}");

            await _publishEndpoint.Publish(new PaymentCompletedEvent(context.Message.CorrelationId));
        }
        else
        {
            _logger.LogInformation($"{context.Message.Payment.TotalPrice} TL was NOT   withdrawn from credit card for buyer id {context.Message.BuyerId}");

            await _publishEndpoint.Publish(new PaymentFailedEvent(context.Message.CorrelationId)
            {
                Reason = "Not withdrawn because of balance",
                OrderItems = context.Message.OrderItems
            });
        }
    }
}
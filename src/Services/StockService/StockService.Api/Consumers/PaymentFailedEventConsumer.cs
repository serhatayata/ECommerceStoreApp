using MassTransit;
using Shared.Queue.Events;
using StockService.Api.Services.Abstract;

namespace StockService.Api.Consumers;

public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
{
    private readonly IStockService _stockService;
    private readonly ILogger<PaymentFailedEventConsumer> _logger;

    public PaymentFailedEventConsumer(
        IStockService stockService, 
        ILogger<PaymentFailedEventConsumer> logger)
    {
        _stockService = stockService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
    {
        foreach (var orderItem in context.Message.OrderItems)
        {
            var stockResult = await _stockService.GetByProductIdAsync(orderItem.ProductId);

            if (stockResult.Success && stockResult.Data != null)
            {
                var result = await _stockService.IncreaseStockAsync(orderItem.ProductId, orderItem.Count);

                if (!result.Success)
                    _logger.LogError("Stock found but NOT INCREASED for Product Id : {0} not found, Buyer Id {1}", orderItem.ProductId, context.Message.BuyerId);
            }
            else
            {
                _logger.LogError("Stock for Product Id : {0} not found", orderItem.ProductId);
            }
        }
    }
}

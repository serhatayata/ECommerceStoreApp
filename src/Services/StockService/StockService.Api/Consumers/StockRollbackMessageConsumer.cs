using MassTransit;
using Shared.Queue.Events;
using Shared.Queue.Messages;
using StockService.Api.Services.Abstract;

namespace StockService.Api.Consumers;

public class StockRollbackMessageConsumer : IConsumer<IStockRollbackMessage>
{
    private readonly IStockService _stockService;
    private readonly ILogger<StockRollbackMessageConsumer> _logger;

    public StockRollbackMessageConsumer(
        IStockService stockService,
        ILogger<StockRollbackMessageConsumer> logger)
    {
        _stockService = stockService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IStockRollbackMessage> context)
    {
        foreach (var orderItem in context.Message.OrderItems)
        {
            var stockResult = await _stockService.GetByProductIdAsync(orderItem.ProductId);

            if (stockResult.Success && stockResult.Data != null)
            {
                var result = await _stockService.IncreaseStockAsync(orderItem.ProductId, orderItem.Count);

                if (!result.Success)
                    _logger.LogError("Stock found but NOT INCREASED for Product Id : {0} not found, Order Id {1}", orderItem.ProductId, context.Message.OrderId);
            }
            else
            {
                _logger.LogError("Stock for Product Id : {0} not found", orderItem.ProductId);
            }
        }
    }
}

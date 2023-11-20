using MassTransit;
using OrderService.Api.Models.Enums;
using OrderService.Api.Services.Abstract;
using Shared.Queue.Events;

namespace OrderService.Api.Consumers;

public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
{
    private readonly IOrderService _orderService;
    private readonly ILogger<StockNotReservedEventConsumer> _logger;

    public StockNotReservedEventConsumer(
        IOrderService orderService, 
        ILogger<StockNotReservedEventConsumer> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
    {
        var order = await _orderService.GetAsync(context.Message.OrderId);

        if (order != null)
        {
            var updateResult = await _orderService.UpdateOrderStatusAsync(context.Message.OrderId, OrderStatus.Fail);

            if (!updateResult.Success)
                _logger.LogError($"Order NOT updated to FAILED for OrderId : {context.Message.OrderId}");
            else
                _logger.LogInformation($"Order status changed to FAILED for OrderId : {context.Message.OrderId}");
        }
        else
        {
            _logger.LogError($"Order NOT FOUND for OrderId : {context.Message.OrderId}");
        }
    }
}

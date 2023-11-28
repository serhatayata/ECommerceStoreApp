using MassTransit;
using OrderService.Api.Models.Enums;
using OrderService.Api.Services.Abstract;
using Shared.Queue.Events.Interfaces;

namespace OrderService.Api.Consumers;

public class OrderFailedRequestEventConsumer : IConsumer<IOrderFailedRequestEvent>
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderCompletedRequestEventConsumer> _logger;

    public OrderFailedRequestEventConsumer(
        IOrderService orderService,
        ILogger<OrderCompletedRequestEventConsumer> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<IOrderFailedRequestEvent> context)
    {
        var order = await _orderService.GetAsync(context.Message.OrderId);

        if (order != null)
        {
            var updateResult = await _orderService.UpdateOrderStatusAsync(context.Message.OrderId, OrderStatus.Fail);

            if (!updateResult.Success)
                _logger.LogError($"Order NOT updated to completed for OrderId : {context.Message.OrderId}");
            else
                _logger.LogInformation($"Order status changed to completed for OrderId : {context.Message.OrderId}");
        }
        else
        {
            _logger.LogError($"Order NOT FOUND for OrderId : {context.Message.OrderId}");
        }
    }
}

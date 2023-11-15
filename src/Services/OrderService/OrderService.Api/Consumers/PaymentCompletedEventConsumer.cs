using AutoMapper;
using MassTransit;
using OrderService.Api.Models.Enums;
using OrderService.Api.Models.OrderModels;
using OrderService.Api.Services.Abstract;
using Shared.Queue.Events;

namespace OrderService.Api.Consumers;

public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;
    private readonly ILogger<PaymentCompletedEventConsumer> _logger;

    public PaymentCompletedEventConsumer(
        IOrderService orderService, 
        IMapper mapper,
        ILogger<PaymentCompletedEventConsumer> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
    {
        var order = await _orderService.GetAsync(context.Message.OrderId);

        if (order != null)
        {
            order.Data.Status = OrderStatus.Completed;
            var updateResult = await _orderService.UpdateOrderStatusAsync(context.Message.OrderId, OrderStatus.Completed);

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

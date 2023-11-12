using AutoMapper;
using MassTransit;
using Shared.Queue.Events;
using StockService.Api.Extensions;
using StockService.Api.Models.StockModels;
using StockService.Api.Services.Abstract;

namespace StockService.Api.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IStockService _stockService;
    private ILogger<OrderCreatedEventConsumer> _logger;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public OrderCreatedEventConsumer(
        IStockService stockService,
        ILogger<OrderCreatedEventConsumer> logger,
        ISendEndpointProvider sendEndpointProvider,
        IPublishEndpoint publishEndpoint,
        IMapper mapper)
    {
        _stockService = stockService;
        _logger = logger;
        _sendEndpointProvider = sendEndpointProvider;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        try
        {
            var stockResult = new Dictionary<int, bool>();

            foreach (var item in context.Message.OrderItems)
            {
                var stock = await _stockService.GetByProductIdAsync(item.ProductId);
                if (stock == null)
                {
                    _logger.LogError($"ProductId : {item.ProductId} - stock not found");
                    continue;
                }

                stockResult.Add(item.ProductId, stock.Data.Count >= item.Count);
            }

            if (stockResult.All(s => s.Value))
            {
                foreach (var item in context.Message.OrderItems)
                {
                    try
                    {
                        var stock = await _stockService.GetByProductIdAsync(item.ProductId);

                        if (stock != null)
                        {
                            var updateResult = await _stockService.DecreaseStockAsync(item.ProductId, item.Count);

                            if (!updateResult.Success)
                                _logger.LogError("OrderCreatedEvent Stock updating not successful for buyerId : {0} - ProductId : {1}",
                                                 context.Message.BuyerId,
                                                 item.ProductId);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("OrderCreatedEvent Stock updating exception : {0}", ex.Message);
                    }
                }

                _logger.LogInformation("Stock was reserved for Buyer Id : {0}", context.Message.BuyerId);

                var stockReservedEventQueueName = MessageBrokerExtensions.GetQueueName<StockReservedEvent>();
                var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{stockReservedEventQueueName}"));

                StockReservedEvent stockReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderId = context.Message.OrderId,
                    OrderItems = context.Message.OrderItems,
                    Payment = context.Message.Payment
                };

                await _sendEndpointProvider.Send(stockReservedEvent);
            }
            else
            {
                await _publishEndpoint.Publish(new StockNotReservedEvent()
                {
                    OrderId = context.Message.OrderId,
                    Message = "Stock not enough"
                });

                _logger.LogInformation("Not enough stock, NOT reserved for Buyer Id : {0}", context.Message.BuyerId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("OrderCreatedEvent Consume exception : {0}", ex.Message);
        }
    }
}

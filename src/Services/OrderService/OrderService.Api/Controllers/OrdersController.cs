using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.DTOs;
using OrderService.Api.Extensions;
using OrderService.Api.Models.OrderModels;
using OrderService.Api.Services.Abstract;
using Shared.Queue.Events;
using Shared.Queue.Events.Interfaces;
using Shared.Queue.Models;

namespace OrderService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        IOrderService orderService,
        ISendEndpointProvider sendEndpointProvider,
        IMapper mapper,
        ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _sendEndpointProvider = sendEndpointProvider;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] OrderCreateDto model)
    {
        var addModel = _mapper.Map<OrderAddModel>(model);
        var result = await _orderService.AddAsync(addModel);
        if (result.Success)
        {
            var orderCreatedRequestEvent = new OrderCreatedRequestEvent()
            {
                BuyerId = model.BuyerId,
                OrderId = result.Data,
                Payment = _mapper.Map<PaymentMessage>(model.Payment, 
                                                      opt => opt.AfterMap((src, dest) =>
                                                      {
                                                          dest.TotalPrice = model.OrderItems.Sum(o => o.Price * o.Count);
                                                      })),
                OrderItems = _mapper.Map<List<OrderItemMessage>>(model.OrderItems)
            };

            var orderCreatedRequestEventName = MessageBrokerExtensions.GetQueueName<OrderCreatedRequestEvent>();
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{orderCreatedRequestEventName}"));

            await sendEndpoint.Send<IOrderCreatedRequestEvent>(orderCreatedRequestEvent);

            return Ok(result);
        }
        return BadRequest(result);
    }
}

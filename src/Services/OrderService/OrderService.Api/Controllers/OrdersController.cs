using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.DTOs;
using OrderService.Api.Models.OrderModels;
using OrderService.Api.Services.Abstract;
using Shared.Queue.Events;
using Shared.Queue.Models;

namespace OrderService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public OrdersController(
        IOrderService orderService,
        IPublishEndpoint publishEndpoint,
        IMapper mapper)
    {
        _orderService = orderService;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create([FromBody] OrderCreateDto model)
    {
        var addModel = _mapper.Map<OrderAddModel>(model);
        var result = await _orderService.AddAsync(addModel);
        if (result.Success)
        {
            var orderCreatedEvent = new OrderCreatedEvent()
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

            await _publishEndpoint.Publish(orderCreatedEvent);

            return Ok(result);
        }
        return BadRequest(result);
    }
}

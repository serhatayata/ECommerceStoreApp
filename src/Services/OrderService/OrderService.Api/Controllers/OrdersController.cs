using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.DTOs;
using OrderService.Api.Models.OrderModels;
using OrderService.Api.Services.Abstract;

namespace OrderService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(
        IOrderService orderService,
        IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(OrderCreateDto model)
    {
        var addModel = _mapper.Map<OrderAddModel>(model);
        var result = await _orderService.AddAsync(addModel);
        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
}

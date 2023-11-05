using Microsoft.AspNetCore.Mvc;
using OrderService.Api.DTOs;

namespace OrderService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(OrderCreateDto model)
    {
        return Ok();
    }
}

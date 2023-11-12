using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockService.Api.Services.Abstract;

namespace StockService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StocksController(
            IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var stocks = await _stockService.GetAllAsync();
            return Ok(stocks);
        }
    }
}

using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(
            IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> AddAsync([FromBody] ProductAddModel model)
        {
            var result = await _productService.AddAsync(model);
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> UpdateAsync([FromBody] ProductUpdateModel model)
        {
            var result = await _productService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> DeleteAsync([FromBody] IntModel model)
        {
            var result = await _productService.DeleteAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(DataResult<ProductModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromBody] IntModel model)
        {
            var result = await _productService.GetAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _productService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-paged")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPagedAsync([FromBody] PagingModel model)
        {
            var result = await _productService.GetAllPagedAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-byproductcode")]
        [ProducesResponseType(typeof(DataResult<ProductModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByProductCodeAsync([FromBody] StringModel model)
        {
            var result = await _productService.GetByProductCodeAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-betweenprices")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllBetweenPricesAsync([FromBody] PriceBetweenModel model)
        {
            var result = await _productService.GetAllBetweenPricesAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-bybrandid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllByBrandIdAsync([FromBody] IntModel model)
        {
            var result = await _productService.GetAllByBrandIdAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-byproducttypeid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllByProductTypeIdAsync([FromBody] IntModel model)
        {
            var result = await _productService.GetAllByProductTypeIdAsync(model);
            return Ok(result);
        }
    }
}

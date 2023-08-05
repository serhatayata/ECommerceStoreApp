using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController<ProductController>
    {
        private readonly IProductService _productService;
        private readonly IRedisService _redisService;

        public ProductController(
            IProductService productService,
            IRedisService redisService)
        {
            _productService = productService;
            _redisService = redisService;
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

        // There might be a lot of products, so this is disabled
        //[HttpGet]
        //[Route("getall")]
        //[ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductModel>>), (int)HttpStatusCode.OK)]
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    var result = await _productService.GetAllAsync();
        //    return Ok(result);
        //}

        [HttpGet]
        [Route("getall-paged")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPagedAsync([FromBody] PagingModel model)
        {
            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName(),
                                                prefix: null,
                                                model.Page.ToString(), model.PageSize.ToString());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<ProductModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _productService.GetAllPagedAsync(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
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
            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName(),
                                                prefix: null,
                                                model.MinimumPrice.ToString(), model.MaximumPrice.ToString());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<ProductModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _productService.GetAllBetweenPricesAsync(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [HttpGet]
        [Route("getall-bybrandid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllByBrandIdAsync([FromBody] IntModel model)
        {
            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName(),
                                                prefix: null,
                                                model.Value.ToString());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<ProductModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _productService.GetAllByBrandIdAsync(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [HttpGet]
        [Route("getall-byproducttypeid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllByProductTypeIdAsync([FromBody] IntModel model)
        {
            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName(),
                                                prefix: null,
                                                model.Value.ToString());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<ProductModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _productService.GetAllByProductTypeIdAsync(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        // PRIVATE METHODS

        [NonAction]
        private async Task RemoveCacheByPattern(CacheType type, params string[] parameters)
        {
            if (type == CacheType.Redis)
            {
                string pattern = string.Join("-", this.ProjectName, this.ClassName, parameters);
                if (parameters.Count() > 0)
                    pattern = string.Join("-", parameters);

                await _redisService.RemoveByPattern(pattern, this.DefaultDatabaseId);
            }
        }
    }
}

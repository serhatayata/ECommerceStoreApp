using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.BrandModels;
using CatalogService.Api.Models.CacheModels;
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
    public class BrandController : BaseController<BrandController>
    {
        private readonly IBrandService _brandService;
        private readonly IRedisService _redisService;

        public BrandController(
            IBrandService brandService,
            IRedisService redisService)
        {
            _brandService = brandService;
            _redisService = redisService;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> AddAsync([FromBody] BrandAddModel model)
        {
            var result = await _brandService.AddAsync(model);
            if (result.Success)
                await this.RemoveCacheByPattern(CacheType.Redis);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> UpdateAsync([FromBody] BrandUpdateModel model)
        {
            var result = await _brandService.UpdateAsync(model);
            if (result.Success)
                await this.RemoveCacheByPattern(CacheType.Redis);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> DeleteAsync([FromBody] IntModel model)
        {
            var result = await _brandService.DeleteAsync(model);
            if (result.Success)
                await this.RemoveCacheByPattern(CacheType.Redis);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(DataResult<BrandModel>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<BrandModel>))]
        public async Task<IActionResult> GetAsync([FromBody] IntModel model)
        {
            var result = await _brandService.GetAsync(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<BrandModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<BrandModel>>))]
        public async Task<IActionResult> GetAllAsync()
        {
            throw new ArgumentNullException();

            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<BrandModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _brandService.GetAllAsync();
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [HttpGet]
        [Route("getall-paged")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<BrandModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<BrandModel>>))]
        public async Task<IActionResult> GetAllPagedAsync([FromBody] PagingModel model)
        {
            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName(), 
                                                prefix: null,
                                                model.Page.ToString(), model.PageSize.ToString());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<BrandModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _brandService.GetAllPagedAsync(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [HttpGet]
        [Route("getall-withproducts")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<BrandModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<BrandModel>>))]
        public async Task<IActionResult> GetAllWithProductsAsync()
        {
            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<BrandModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _brandService.GetAllWithProductsAsync();
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

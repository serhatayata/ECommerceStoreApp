using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.BrandModels;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Models.CategoryModels;
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
    public class CategoryController : BaseController<CategoryController>
    {
        private readonly ICategoryService _categoryService;
        private readonly IRedisService _redisService;

        public CategoryController(
            ICategoryService categoryService,
            IRedisService redisService)
        {
            _categoryService = categoryService;
            _redisService = redisService;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> AddAsync([FromBody] CategoryAddModel model)
        {
            var result = await _categoryService.AddAsync(model);
            if (result.Success)
                await this.RemoveCacheByPattern(CacheType.Redis);

            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> UpdateAsync([FromBody] CategoryUpdateModel model)
        {
            var result = await _categoryService.UpdateAsync(model);
            if (result.Success)
                await this.RemoveCacheByPattern(CacheType.Redis);

            return Ok(result);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> DeleteAsync([FromBody] IntModel model)
        {
            var result = await _categoryService.DeleteAsync(model);
            if (result.Success)
                await this.RemoveCacheByPattern(CacheType.Redis);

            return Ok(result);
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(DataResult<CategoryModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromBody] IntModel model)
        {
            var result = await _categoryService.GetAsync(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CategoryModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<CategoryModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _categoryService.GetAllAsync();
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [HttpGet]
        [Route("getall-byparentid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CategoryModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllByParentId([FromBody] IntModel model)
        {
            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName(),
                                                parameters: model.Value.ToString());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<CategoryModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _categoryService.GetAllByParentId(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [HttpGet]
        [Route("getall-paged")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CategoryModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPagedAsync([FromBody] PagingModel model)
        {
            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName(),
                                                prefix: null,
                                                model.Page.ToString(), model.PageSize.ToString());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<CategoryModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _categoryService.GetAllPagedAsync(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [HttpGet]
        [Route("getall-withproducts-byparentid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CategoryModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllWithProductsByParentId([FromBody] IntModel model)
        {
            var cacheKey = this.CurrentCacheKey(methodName: this.GetActualAsyncMethodName(),
                                                model.Value.ToString());
            var cacheResult = await _redisService.GetAsync<DataResult<IReadOnlyList<CategoryModel>>>(
                cacheKey,
                this.DefaultDatabaseId,
                this.DefaultCacheDuration, async () =>
                {
                    var result = await _categoryService.GetAllWithProductsByParentId(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [HttpGet]
        [Route("get-byname")]
        [ProducesResponseType(typeof(DataResult<CategoryModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByName([FromBody] StringModel model)
        {
            var result = await _categoryService.GetByName(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-byname-withproducts")]
        [ProducesResponseType(typeof(DataResult<CategoryModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByNameWithProducts([FromBody] StringModel model)
        {
            var result = await _categoryService.GetByNameWithProducts(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-withproducts")]
        [ProducesResponseType(typeof(DataResult<CategoryModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetWithProducts([FromBody] IntModel model)
        {
            var result = await _categoryService.GetWithProducts(model);
            return Ok(result);
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

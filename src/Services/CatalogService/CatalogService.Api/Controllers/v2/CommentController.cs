using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Models.CommentModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace CatalogService.Api.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    [ApiController]
    public class CommentController : BaseController<CategoryController>
    {
        private readonly ICommentService _commentService;
        private readonly IRedisService _redisService;

        public CommentController(
            ICommentService commentService,
            IRedisService redisService)
        {
            _commentService = commentService;
            _redisService = redisService;
        }

        [MapToApiVersion("2.0")]
        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> AddAsync([FromBody] CommentAddModel model)
        {
            var result = await _commentService.AddAsync(model);
            if (result.Success)
                await RemoveCacheByPattern(CacheType.Redis);

            return Ok(result);
        }

        [MapToApiVersion("2.0")]
        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> UpdateAsync([FromBody] CommentUpdateModel model)
        {
            var result = await _commentService.UpdateAsync(model);
            if (result.Success)
                await RemoveCacheByPattern(CacheType.Redis);

            return Ok(result);
        }

        [MapToApiVersion("2.0")]
        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> DeleteAsync([FromBody] IntModel model)
        {
            var result = await _commentService.DeleteAsync(model);
            if (result.Success)
                await RemoveCacheByPattern(CacheType.Redis);

            return Ok(result);
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(DataResult<CommentModel>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<CommentModel>))]
        public async Task<IActionResult> GetAsync([FromBody] IntModel model)
        {
            var result = await _commentService.GetAsync(model);
            return Ok(result);
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("get-bycode")]
        [ProducesResponseType(typeof(DataResult<CommentModel>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<CommentModel>))]
        public async Task<IActionResult> GetByCodeAsync([FromBody] StringModel model)
        {
            var result = await _commentService.GetByCodeAsync(model);
            return Ok(result);
        }

        //There can be more data than expected comment when we get all. So disabled for now
        //[HttpGet]
        //[Route("getall")]
        //[ProducesResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>), (int)HttpStatusCode.OK)]
        //[ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>))]
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    var result = await _commentService.GetAllAsync();
        //    return Ok(result);
        //}

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("getall-paged")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>))]
        public async Task<IActionResult> GetAllPagedAsync([FromBody] PagingModel model)
        {
            var cacheKey = CurrentCacheKey(methodName: GetActualAsyncMethodName(),
                                                prefix: null,
                                                model.Page.ToString(), model.PageSize.ToString());
            var cacheResult = await _redisService.GetAsync(
                cacheKey,
                DefaultDatabaseId,
                DefaultCacheDuration, async () =>
                {
                    var result = await _commentService.GetAllPagedAsync(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("getall-byproductid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>))]
        public async Task<IActionResult> GetAllByProductIdAsync([FromBody] IntModel model)
        {
            var cacheKey = CurrentCacheKey(methodName: GetActualAsyncMethodName(),
                                                model.Value.ToString());
            var cacheResult = await _redisService.GetAsync(
                cacheKey,
                DefaultDatabaseId,
                DefaultCacheDuration, async () =>
                {
                    var result = await _commentService.GetAllByProductId(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("getall-byproductcode")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>))]
        public async Task<IActionResult> GetAllByProductCodeAsync([FromBody] IntModel model)
        {
            var cacheKey = CurrentCacheKey(methodName: GetActualAsyncMethodName(),
                                                model.Value.ToString());
            var cacheResult = await _redisService.GetAsync(
                cacheKey,
                DefaultDatabaseId,
                DefaultCacheDuration, async () =>
                {
                    var result = await _commentService.GetAllByProductCode(model);
                    return result;
                });

            return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
        }

        [MapToApiVersion("2.0")]
        [HttpGet]
        [Route("getall-byuserid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>))]
        public async Task<IActionResult> GetAllByUserIdAsync([FromBody] StringModel model)
        {
            var result = await _commentService.GetAllByUserId(model);
            return Ok(result);
        }

        // PRIVATE METHODS

        [NonAction]
        private async Task RemoveCacheByPattern(CacheType type, params string[] parameters)
        {
            if (type == CacheType.Redis)
            {
                string pattern = string.Join("-", ProjectName, ClassName, parameters);
                if (parameters.Count() > 0)
                    pattern = string.Join("-", parameters);

                await _redisService.RemoveByPattern(pattern, DefaultDatabaseId);
            }
        }
    }
}

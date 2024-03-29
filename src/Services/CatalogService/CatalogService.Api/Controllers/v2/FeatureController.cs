﻿using CatalogService.Api.Attributes;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Models.CommentModels;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Reflection;

namespace CatalogService.Api.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("2.0")]
[ApiController]
public class FeatureController : BaseController<FeatureController>
{
    private readonly IFeatureService _featureService;
    private readonly IRedisService _redisService;

    public FeatureController(
        IFeatureService featureService,
        IRedisService redisService)
    {
        _featureService = featureService;
        _redisService = redisService;
    }

    [AuthorizeMultiplePolicy("CatalogWrite", false)]
    [MapToApiVersion("2.0")]
    [HttpPost]
    [Route("add")]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Result))]
    public async Task<IActionResult> AddAsync([FromBody] FeatureAddModel model)
    {
        var result = await _featureService.AddAsync(model);
        if (result.Success)
            await RemoveCacheByPattern(CacheType.Redis);

        return Ok(result);
    }

    [AuthorizeMultiplePolicy("CatalogWrite", false)]
    [MapToApiVersion("2.0")]
    [HttpPut]
    [Route("update")]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Result))]
    public async Task<IActionResult> UpdateAsync([FromBody] FeatureUpdateModel model)
    {
        var result = await _featureService.UpdateAsync(model);
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("CatalogWrite", false)]
    [MapToApiVersion("2.0")]
    [HttpDelete]
    [Route("delete")]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Result))]
    public async Task<IActionResult> DeleteAsync([FromBody] IntModel model)
    {
        var result = await _featureService.DeleteAsync(model);
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("CatalogWrite", false)]
    [MapToApiVersion("2.0")]
    [HttpPost]
    [Route("add-productfeature")]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Result))]
    public async Task<IActionResult> AddProductFeatureAsync([FromBody] ProductFeatureModel model)
    {
        var result = await _featureService.AddProductFeatureAsync(model);
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("CatalogWrite", false)]
    [MapToApiVersion("2.0")]
    [HttpDelete]
    [Route("delete-productfeature")]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Result))]
    public async Task<IActionResult> DeleteProductFeatureAsync([FromBody] ProductFeatureModel model)
    {
        var result = await _featureService.DeleteProductFeatureAsync(model);
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("CatalogWrite", false)]
    [MapToApiVersion("2.0")]
    [HttpPost]
    [Route("add-productfeature-property")]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Result))]
    public async Task<IActionResult> AddProductFeaturePropertyAsync([FromBody] ProductFeaturePropertyAddModel model)
    {
        var result = await _featureService.AddProductFeaturePropertyAsync(model);
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("CatalogWrite", false)]
    [MapToApiVersion("2.0")]
    [HttpPut]
    [Route("update-productfeature-property")]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Result))]
    public async Task<IActionResult> UpdateProductFeaturePropertyAsync([FromBody] ProductFeaturePropertyUpdateModel model)
    {
        var result = await _featureService.UpdateProductFeaturePropertyAsync(model);
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("CatalogWrite", false)]
    [MapToApiVersion("2.0")]
    [HttpDelete]
    [Route("delete-productfeature-property")]
    [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
    [ProducesErrorResponseType(typeof(Result))]
    public async Task<IActionResult> DeleteProductFeaturePropertyAsync([FromBody] IntModel model)
    {
        var result = await _featureService.DeleteProductFeaturePropertyAsync(model);
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("CatalogRead", false)]
    [MapToApiVersion("2.0")]
    [HttpGet]
    [Route("get")]
    [ProducesResponseType(typeof(DataResult<FeatureModel>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAsync([FromBody] IntModel model)
    {
        var result = await _featureService.GetAsync(model);
        return Ok(result);
    }

    [AuthorizeMultiplePolicy("CatalogRead", false)]
    [MapToApiVersion("2.0")]
    [HttpGet]
    [Route("getall")]
    [ProducesResponseType(typeof(DataResult<IReadOnlyList<FeatureModel>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllAsync()
    {
        var cacheKey = CurrentCacheKey(methodName: GetActualAsyncMethodName());
        var cacheResult = await _redisService.GetAsync(
            cacheKey,
            DefaultDatabaseId,
            DefaultCacheDuration, async () =>
            {
                var result = await _featureService.GetAllAsync();
                return result;
            });

        return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
    }

    [AuthorizeMultiplePolicy("CatalogRead", false)]
    [MapToApiVersion("2.0")]
    [HttpGet]
    [Route("getall-paged")]
    [ProducesResponseType(typeof(DataResult<IReadOnlyList<FeatureModel>>), (int)HttpStatusCode.OK)]
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
                var result = await _featureService.GetAllPagedAsync(model);
                return result;
            });

        return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
    }

    [AuthorizeMultiplePolicy("CatalogRead", false)]
    [MapToApiVersion("2.0")]
    [HttpGet]
    [Route("getall-features-byproductid")]
    [ProducesResponseType(typeof(DataResult<IReadOnlyList<FeatureModel>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllFeaturesByProductId([FromBody] IntModel model)
    {
        var cacheKey = CurrentCacheKey(methodName: GetActualAsyncMethodName(),
                                            parameters: model.Value.ToString());
        var cacheResult = await _redisService.GetAsync(
            cacheKey,
            DefaultDatabaseId,
            DefaultCacheDuration, async () =>
            {
                var result = await _featureService.GetAllFeaturesByProductId(model);
                return result;
            });

        return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
    }

    [AuthorizeMultiplePolicy("CatalogRead", false)]
    [MapToApiVersion("2.0")]
    [HttpGet]
    [Route("getall-featureproperties")]
    [ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductFeaturePropertyModel>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllFeatureProperties([FromBody] ProductFeatureModel model)
    {
        var cacheKey = CurrentCacheKey(methodName: GetActualAsyncMethodName(),
                                            prefix: null,
                                            model.ProductId.ToString(), model.FeatureId.ToString());
        var cacheResult = await _redisService.GetAsync(
            cacheKey,
            DefaultDatabaseId,
            DefaultCacheDuration, async () =>
            {
                var result = await _featureService.GetAllFeatureProperties(model);
                return result;
            });

        return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
    }

    [AuthorizeMultiplePolicy("CatalogRead", false)]
    [MapToApiVersion("2.0")]
    [HttpGet]
    [Route("getall-featureproperties-byproductfeatureid")]
    [ProducesResponseType(typeof(DataResult<IReadOnlyList<ProductFeaturePropertyModel>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllFeaturePropertiesByProductFeatureId([FromBody] IntModel model)
    {
        var cacheKey = CurrentCacheKey(methodName: GetActualAsyncMethodName(),
                                            prefix: null,
                                            model.Value.ToString());
        var cacheResult = await _redisService.GetAsync(
            cacheKey,
            DefaultDatabaseId,
            DefaultCacheDuration, async () =>
            {
                var result = await _featureService.GetAllFeaturePropertiesByProductFeatureId(model);
                return result;
            });

        return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
    }

    [AuthorizeMultiplePolicy("CatalogRead", false)]
    [MapToApiVersion("2.0")]
    [HttpGet]
    [Route("getall-features-byproductcode")]
    [ProducesResponseType(typeof(DataResult<IReadOnlyList<FeatureModel>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetAllFeaturesByProductCode([FromBody] StringModel model)
    {
        var cacheKey = CurrentCacheKey(methodName: GetActualAsyncMethodName(),
                                            prefix: null,
                                            model.Value);
        var cacheResult = await _redisService.GetAsync(
            cacheKey,
            DefaultDatabaseId,
            DefaultCacheDuration, async () =>
            {
                var result = await _featureService.GetAllFeaturesByProductCode(model);
                return result;
            });

        return cacheResult.Success ? Ok(cacheResult) : BadRequest(cacheResult);
    }

    [AuthorizeMultiplePolicy("CatalogRead", false)]
    [MapToApiVersion("2.0")]
    [HttpGet]
    [Route("get-featureproducts")]
    [ProducesResponseType(typeof(DataResult<IReadOnlyList<Product>>), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetFeatureProducts([FromBody] IntModel model)
    {
        var cacheKey = CurrentCacheKey(methodName: GetActualAsyncMethodName(),
                                            prefix: null,
                                            model.Value.ToString());
        var cacheResult = await _redisService.GetAsync(
            cacheKey,
            DefaultDatabaseId,
            DefaultCacheDuration, async () =>
            {
                var result = await _featureService.GetFeatureProducts(model);
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
            string pattern = string.Join("-", ProjectName, ClassName, parameters);
            if (parameters.Count() > 0)
                pattern = string.Join("-", parameters);

            await _redisService.RemoveByPattern(pattern, DefaultDatabaseId);
        }
    }
}

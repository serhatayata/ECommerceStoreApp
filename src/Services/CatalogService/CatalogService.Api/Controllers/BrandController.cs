using AutoMapper;
using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.BrandModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : BaseController
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddAsync(BrandAddModel model)
        {
            var result = await _brandService.AddAsync(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync(BrandUpdateModel model)
        {
            var result = await _brandService.UpdateAsync(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync(IntModel model)
        {
            var result = await _brandService.DeleteAsync(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync(IntModel model)
        {
            var result = await _brandService.GetAsync(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _brandService.GetAllAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall-paged")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPagedAsync(PagingModel model)
        {
            var result = await _brandService.GetAllPagedAsync(model);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet]
        [Route("getall-withproducts")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllWithProductsAsync()
        {
            var result = await _brandService.GetAllWithProductsAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}

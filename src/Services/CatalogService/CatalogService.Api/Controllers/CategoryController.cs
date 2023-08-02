using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CategoryModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> AddAsync([FromBody] CategoryAddModel model)
        {
            var result = await _categoryService.AddAsync(model);
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> UpdateAsync([FromBody] CategoryUpdateModel model)
        {
            var result = await _categoryService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> DeleteAsync([FromBody] IntModel model)
        {
            var result = await _categoryService.DeleteAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(DataResult<CategoryModel>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAsync([FromBody] IntModel model)
        {
            var result = await _categoryService.GetAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CategoryModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _categoryService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-byparentid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CategoryModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllByParentId([FromBody] IntModel model)
        {
            var result = await _categoryService.GetAllByParentId(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-paged")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CategoryModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllPagedAsync([FromBody] PagingModel model)
        {
            var result = await _categoryService.GetAllPagedAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-withproducts-byparentid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CategoryModel>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllWithProductsByParentId([FromBody] IntModel model)
        {
            var result = await _categoryService.GetAllWithProductsByParentId(model);
            return Ok(result);
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
    }
}

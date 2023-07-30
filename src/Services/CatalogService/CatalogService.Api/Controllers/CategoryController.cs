using CatalogService.Api.Models.CategoryModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        //[HttpPost]
        //[Route("add")]
        //[ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        //[ProducesErrorResponseType(typeof(Result))]
        //public async Task<IActionResult> AddAsync(CategoryAddModel model)
        //{
            


        //}
    }
}

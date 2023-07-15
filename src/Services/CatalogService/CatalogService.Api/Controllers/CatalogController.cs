using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Models.Base.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CatalogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            int x = 1;
            int y = 4;
            var result = await _unitOfWork.DapperCategoryRepository.AddAsync(new Entities.Category()
            {
                UpdateDate = DateTime.Now,
                Line = 2,
                Name = "cat2c",
                Link = "cat2c",
                ParentId = 2
            });

            return Ok(result);
        }
    }
}

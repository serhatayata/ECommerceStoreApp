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
        private readonly IDapperBrandRepository _brandRepository;

        public CatalogController(IUnitOfWork unitOfWork, IDapperBrandRepository dapperBrandRepository)
        {
            _unitOfWork = unitOfWork;
            _brandRepository = dapperBrandRepository;
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            int x = 1;
            int y = 4;
            var result = await _brandRepository.UpdateAsync(new Entities.Brand()
            {
                Id = 1,
                Name = "test2.1",
                Description = "test descr 2.1"
            });

            return Ok(result);
        }
    }
}

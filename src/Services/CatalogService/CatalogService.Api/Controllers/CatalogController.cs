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
        private readonly IDapperFeatureRepository _dapperFeatureRepository;

        public CatalogController(IDapperFeatureRepository dapperFeatureRepository)
        {
            _dapperFeatureRepository = dapperFeatureRepository;
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            int x = 1;
            int y = 4;
            var result = await _dapperFeatureRepository.GetAllFeaturePropertiesByProductFeatureId(new IntModel() { Value = 2 });
            return Ok(result);
        }
    }
}

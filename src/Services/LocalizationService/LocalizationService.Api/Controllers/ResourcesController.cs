using LocalizationService.Api.Data.Repositories.Dapper.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IDapperResourceRepository _resourceRepository;

        public ResourcesController(IDapperResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _resourceRepository.GetAsync(new Models.Base.Concrete.StringModel() { Value = "f7822873-3e1e-4a68-b86d-96766b92c6ab" });
            return Ok(result);
        }
    }
}

using LocalizationService.Api.Data.Repositories.Dapper.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly IDapperLanguageRepository _languageRepository;

        public LanguagesController(IDapperLanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _languageRepository.GetAllWithResourcesAsync();
            return Ok(result);
        }
    }
}

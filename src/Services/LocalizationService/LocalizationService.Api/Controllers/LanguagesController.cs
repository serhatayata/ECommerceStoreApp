using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.LanguageModels;
using LocalizationService.Api.Services.Abstract;
using LocalizationService.Api.Utilities.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguagesController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(SuccessResult), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResult), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(Policy = "LocalizationWrite")]
        public async Task<IActionResult> AddAsync([FromBody] LanguageAddModel model)
        {
            var result = await _languageService.AddAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(SuccessResult), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResult), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(Policy = "LocalizationWrite")]
        public async Task<IActionResult> UpdateAsync([FromBody] LanguageUpdateModel model)
        {
            var result = await _languageService.UpdateAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(SuccessResult), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResult), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(Policy = "LocalizationWrite")]
        public async Task<IActionResult> DeleteAsync([FromBody] StringModel model)
        {
            var result = await _languageService.DeleteAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get")]
        [ProducesResponseType(typeof(SuccessDataResult<LanguageModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorDataResult<LanguageModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(Policy = "LocalizationRead")]
        public async Task<IActionResult> GetAsync([FromBody] StringModel model)
        {
            var result = await _languageService.GetAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all")]
        [ProducesResponseType(typeof(DataResult<LanguageModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<LanguageModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(Policy = "LocalizationRead")]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _languageService.GetAllAsync();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all-paging")]
        [ProducesResponseType(typeof(DataResult<LanguageModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<LanguageModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(Policy = "LocalizationRead")]
        public async Task<IActionResult> GetAllPagingAsync([FromBody] PagingModel model)
        {
            var result = await _languageService.GetAllPagingAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all-with-resources")]
        [ProducesResponseType(typeof(DataResult<LanguageModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<LanguageModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(Policy = "LocalizationRead")]
        public async Task<IActionResult> GetAllWithResourcesAsync()
        {
            var result = await _languageService.GetAllWithResourcesAsync();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all-with-resources-paging")]
        [ProducesResponseType(typeof(DataResult<LanguageModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<LanguageModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        [Authorize(Policy = "LocalizationRead")]
        public async Task<IActionResult> GetAllWithResourcesPagingAsync([FromBody] PagingModel model)
        {
            var result = await _languageService.GetAllWithResourcesPagingAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}

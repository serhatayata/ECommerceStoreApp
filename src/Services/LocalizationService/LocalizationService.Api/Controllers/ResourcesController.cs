using LocalizationService.Api.Data.Repositories.Dapper.Abstract;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.MemberModels;
using LocalizationService.Api.Models.ResourceModels;
using LocalizationService.Api.Services.Abstract;
using LocalizationService.Api.Services.Concrete;
using LocalizationService.Api.Utilities.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourceService _resourceService;

        public ResourcesController(IResourceService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(SuccessResult), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResult), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] ResourceAddModel model)
        {
            var result = await _resourceService.AddAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(SuccessResult), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResult), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] ResourceUpdateModel model)
        {
            var result = await _resourceService.UpdateAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(SuccessResult), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResult), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromBody] StringModel model)
        {
            var result = await _resourceService.DeleteAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all")]
        [ProducesResponseType(typeof(DataResult<ResourceModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<ResourceModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _resourceService.GetAllAsync();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all-paging")]
        [ProducesResponseType(typeof(DataResult<ResourceModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<ResourceModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllPagingAsync([FromBody] PagingModel model)
        {
            var result = await _resourceService.GetAllPagingAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all-active")]
        [ProducesResponseType(typeof(DataResult<ResourceModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<ResourceModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllActiveAsync()
        {
            var result = await _resourceService.GetAllActiveAsync();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all-active-paging")]
        [ProducesResponseType(typeof(DataResult<ResourceModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<ResourceModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllActivePagingAsync([FromBody] PagingModel model)
        {
            var result = await _resourceService.GetAllActivePagingAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}

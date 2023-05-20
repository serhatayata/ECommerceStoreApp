using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.MemberModels;
using LocalizationService.Api.Services.Abstract;
using LocalizationService.Api.Utilities.Results;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(SuccessResult), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResult), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAsync([FromBody] MemberAddModel model)
        {
            var result = await _memberService.AddAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(SuccessResult), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResult), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateAsync([FromBody] MemberUpdateModel model)
        {
            var result = await _memberService.UpdateAsync(model);
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
            var result = await _memberService.DeleteAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all")]
        [ProducesResponseType(typeof(DataResult<MemberModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<MemberModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _memberService.GetAllAsync();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all-paging")]
        [ProducesResponseType(typeof(DataResult<MemberModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<MemberModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllPagingAsync([FromBody] PagingModel model)
        {
            var result = await _memberService.GetAllPagingAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all-with-resources")]
        [ProducesResponseType(typeof(DataResult<MemberModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<MemberModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllWithResourcesAsync()
        {
            var result = await _memberService.GetAllWithResourcesAsync();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost]
        [Route("get-all-with-resources-paging")]
        [ProducesResponseType(typeof(DataResult<MemberModel>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType(typeof(DataResult<MemberModel>), (int)System.Net.HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllWithResourcesPagingAsync([FromBody] PagingModel model)
        {
            var result = await _memberService.GetAllWithResourcesPagingAsync(model);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}

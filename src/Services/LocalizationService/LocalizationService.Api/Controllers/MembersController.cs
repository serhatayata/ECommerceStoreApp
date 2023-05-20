using LocalizationService.Api.Data.Repositories.Dapper.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalizationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IDapperMemberRepository _memberRepository;

        public MembersController(IDapperMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _memberRepository.GetAsync(new Models.Base.Concrete.StringModel() { Value = "f7822873-3e1e-4a68-b86d-96766b92c6ab" });
            return Ok(result);
        }
    }
}

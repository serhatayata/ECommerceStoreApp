using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Models.Email;
using NotificationService.Api.Services.Base.Abstract;

namespace NotificationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(
            IEmailService emailService)
        {
            _emailService = emailService;
        }

        //[Authorize("NotificationWrite")]
        [HttpPost]
        [Route("send-order-success-email")]
        public async Task<IActionResult> SendOrderSuccessEmail(OrderSuccessEmail model)
        {
            var result = await _emailService.SendOrderSuccessEmail(model);
            return Ok(result);
        }
    }
}

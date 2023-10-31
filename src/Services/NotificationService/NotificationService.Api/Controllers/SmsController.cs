using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Models.Email;
using NotificationService.Api.Models.Sms;
using NotificationService.Api.Services.Base.Abstract;

namespace NotificationService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _smsService;

        public SmsController(
            ISmsService smsService)
        {
            _smsService = smsService;
        }

        //[Authorize("NotificationWrite")]
        [HttpPost]
        [Route("send-order-success-sms")]
        public async Task<IActionResult> SendOrderSuccessEmail(OrderSuccessSMS model)
        {
            var result = await _smsService.SendOrderSuccessSMS(model);
            return Ok(result);
        }
    }
}

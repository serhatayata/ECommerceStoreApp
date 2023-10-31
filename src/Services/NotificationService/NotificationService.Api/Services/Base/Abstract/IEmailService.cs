using NotificationService.Api.Models.Email;
using NotificationService.Api.Models.Sms;

namespace NotificationService.Api.Services.Base.Abstract;

public interface IEmailService
{
    ValueTask<bool> SendOrderSuccessEmail(OrderSuccessEmail emailData);
}

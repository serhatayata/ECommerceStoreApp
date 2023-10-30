using NotificationService.Api.Models.Email;

namespace NotificationService.Api.Services.Base.Abstract;

public interface IEmailService
{
    ValueTask<bool> SendSmtpEmail(EmailBase emailData);
    ValueTask<bool> SendSmtpEmailWithAttachment(EmailWithAttachment emailData);
}

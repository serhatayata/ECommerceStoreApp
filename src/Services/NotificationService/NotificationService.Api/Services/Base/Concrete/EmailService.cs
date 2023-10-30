using NotificationService.Api.Models.Email;
using NotificationService.Api.Services.Base.Abstract;
using NotificationService.Api.Services.Cache.Abstract;

namespace NotificationService.Api.Services.Base.Concrete;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly ILocalizationService _localizationService;

    public EmailService(
        ILogger<EmailService> logger, 
        ILocalizationService localizationService)
    {
        _logger = logger;
        _localizationService = localizationService;
    }

    public async ValueTask<bool> SendSmtpEmail(EmailBase emailData)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<bool> SendSmtpEmailWithAttachment(EmailWithAttachment emailData)
    {
        throw new NotImplementedException();
    }
}

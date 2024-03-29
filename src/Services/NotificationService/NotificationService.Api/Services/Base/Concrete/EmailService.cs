﻿using Microsoft.Extensions.Options;
using NotificationService.Api.Models.Email;
using NotificationService.Api.Models.Settings;
using NotificationService.Api.Services.Base.Abstract;
using System.Reflection;

namespace NotificationService.Api.Services.Base.Concrete;

public class EmailService : BaseService, IEmailService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        ILogger<EmailService> logger, 
        IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async ValueTask<bool> SendOrderSuccessEmail(OrderSuccessEmail emailData)
    {
        try
        {
            var emailBody = this.GetLocalizedValue("BC22U.EmailSuccess");
            var emailSubject = this.GetLocalizedValue("BC22U.EmailSuccessSubject");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                    ex.Message, nameof(EmailService),
                    MethodBase.GetCurrentMethod()?.Name);
            return false;
        }
    }
}

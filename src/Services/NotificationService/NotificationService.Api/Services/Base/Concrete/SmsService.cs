using NotificationService.Api.Models.Sms;
using NotificationService.Api.Services.Base.Abstract;
using System.Reflection;

namespace NotificationService.Api.Services.Base.Concrete;

public class SmsService : BaseService, ISmsService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<SmsService> _logger;

    public SmsService(
        ILogger<SmsService> logger,
        IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async ValueTask<bool> SendOrderSuccessSMS(OrderSuccessSMS emailData)
    {
        try
        {
            var smsBody = this.GetLocalizedValue("BC22U.SmsSuccess");
            var smsSubject = this.GetLocalizedValue("BC22U.SmsSuccessSubject");
            throw new Exception();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("ERROR handling message: {ExceptionMessage} - Method : {ClassName}.{MethodName}",
                    ex.Message, nameof(SmsService),
                    MethodBase.GetCurrentMethod()?.Name);
            return false;
        }
    }
}

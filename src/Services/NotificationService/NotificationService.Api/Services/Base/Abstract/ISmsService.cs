using NotificationService.Api.Models.Sms;
using NotificationService.Api.Services.Base.Concrete;
using System.Reflection;

namespace NotificationService.Api.Services.Base.Abstract;

public interface ISmsService
{
    ValueTask<bool> SendOrderSuccessSMS(OrderSuccessSMS emailData);
}

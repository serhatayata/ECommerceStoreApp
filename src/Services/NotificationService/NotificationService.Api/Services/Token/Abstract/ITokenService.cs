using NotificationService.Api.Models.IdentityModels;
using NotificationService.Api.Utilities.Results;

namespace NotificationService.Api.Services.Token.Abstract;

public interface ITokenService
{
    Task<DataResult<string>> GetToken(ClientCredentialsTokenModel model);
}

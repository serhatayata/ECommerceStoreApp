using MonitoringService.Api.Models.IdentityModels;
using MonitoringService.Api.Utilities.Results;

namespace MonitoringService.Api.Services.Token.Abstract;

public interface ITokenService
{
    Task<DataResult<string>> GetToken(ClientCredentialsTokenModel model);
}

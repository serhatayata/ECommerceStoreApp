using Monitoring.BackgroundTasks.Models.IdentityModels;
using Monitoring.BackgroundTasks.Utilities.Enums;
using Monitoring.BackgroundTasks.Utilities.Results;

namespace Monitoring.BackgroundTasks.Services.Abstract;

public interface ITokenService
{
    Task<DataResult<string>> GetToken(ClientCredentialsTokenModel model);
}

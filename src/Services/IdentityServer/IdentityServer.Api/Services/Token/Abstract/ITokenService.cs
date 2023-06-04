using IdentityServer.Api.Utilities.Enums;
using IdentityServer.Api.Utilities.Results;

namespace IdentityServer.Api.Services.Token.Abstract
{
    public interface ITokenService
    {
        Task<DataResult<string>> GetToken(EnumProjectType type);
    }
}

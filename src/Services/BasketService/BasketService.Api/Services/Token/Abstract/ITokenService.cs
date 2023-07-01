using BasketService.Api.Utilities.Enums;
using BasketService.Api.Utilities.Results;

namespace BasketService.Api.Services.Token.Abstract
{
    public interface ITokenService
    {
        Task<DataResult<string>> GetToken(EnumProjectType type, ApiPermissionType permissionType);
    }
}

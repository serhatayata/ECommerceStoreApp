using BasketGrpcService.Utilities.Enums;
using BasketGrpcService.Utilities.Results;

namespace BasketGrpcService.Services.Token.Abstract
{
    public interface ITokenService
    {
        Task<DataResult<string>> GetToken(EnumProjectType type, ApiPermissionType permissionType);
    }
}

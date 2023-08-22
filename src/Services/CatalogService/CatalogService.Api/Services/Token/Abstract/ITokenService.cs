using CatalogService.Api.Utilities.Enums;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Token.Abstract;
public interface ITokenService
{
    Task<DataResult<string>> GetToken(EnumProjectType type, ApiPermissionType permissionType);
}

using BasketGrpcService.Services.Token.Abstract;
using BasketGrpcService.Utilities.Enums;
using BasketGrpcService.Utilities.Results;

namespace BasketGrpcService.Services.Token.Concrete
{
    public class ClientCredentialsTokenService : IClientCredentialsTokenService
    {
        /// <summary>
        /// This method gets only token for itself from IdentityServer
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<DataResult<string>> GetToken(EnumProjectType type, ApiPermissionType permissionType)
        {
            return null;
        }
    }
}

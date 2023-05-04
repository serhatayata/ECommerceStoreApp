using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Utilities.Security.Jwt.Models;
using System.Security.Claims;

namespace IdentityServer.Api.Utilities.Security.Jwt
{
    public interface IJwtHelper
    {
        JwtAccessToken CreateToken(User user, List<Claim> operationClaims, bool containsRefreshToken);
    }
}

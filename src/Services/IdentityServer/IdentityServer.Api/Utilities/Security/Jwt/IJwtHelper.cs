using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Utilities.Results;
using IdentityServer.Api.Utilities.Security.Jwt.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace IdentityServer.Api.Utilities.Security.Jwt
{
    public interface IJwtHelper
    {
        /// <summary>
        /// Create new jwt token
        /// </summary>
        /// <param name="user">user to get claims and roles</param>
        /// <param name="operationClaims">user claims</param>
        /// <param name="containsRefreshToken">if contains refresh token or not</param>
        /// <returns></returns>
        JwtAccessToken CreateToken(User user, List<Claim> operationClaims, int expiration,bool containsRefreshToken);
        /// <summary>
        /// Validate token
        /// </summary>
        /// <param name="token">jwt token</param>
        /// <param name="scheme">current authentication scheme</param>
        /// <returns></returns>
        DataResult<List<Claim>> ValidateCurrentToken(string token, string scheme);
    }
}

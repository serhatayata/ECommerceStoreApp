using IdentityModel;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Utilities.Security.Jwt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityServer.Api.Utilities.Security.Jwt
{
    public class JwtHelper : IJwtHelper
    {
        private IConfiguration Configuration { get; }
        private readonly JwtTokenOptions _tokenOptions;

        public JwtHelper(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("JwtTokenOptionsForVerify").Get<JwtTokenOptions>(); ;
        }

        public JwtAccessToken CreateToken(User user, List<Claim> operationClaims, bool containsRefreshToken)
        {
            int expiration = _tokenOptions.AccessTokenExpiration;
            int refreshExpiration = _tokenOptions.RefreshTokenExpiration;

            string issuer = _tokenOptions.Issuer;
            string audience = _tokenOptions.Audience;
            string secretKey = _tokenOptions.SecretKey;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = CreateJwtSecurityToken(issuer, 
                                                          audience, 
                                                          expiration,
                                                          user, 
                                                          signingCredentials, 
                                                          operationClaims);

            var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            string refreshToken = null;
            if (containsRefreshToken)
            {
                var refreshSecurityToken = CreateJwtSecurityToken(issuer,
                                                                  audience,
                                                                  refreshExpiration,
                                                                  user,
                                                                  signingCredentials,
                                                                  operationClaims);

                refreshToken = jwtSecurityTokenHandler.WriteToken(refreshSecurityToken);
            }

            return new JwtAccessToken
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                Expiration = DateTime.Now.AddMinutes(expiration),
            };
        }

        private JwtSecurityToken CreateJwtSecurityToken(string issuer, 
                                                        string audience,
                                                        int expiration,
                                                        User user,
                                                        SigningCredentials signingCredentials, 
                                                        List<Claim> operationClaims)
        {
            var tokenExpiration = DateTime.Now.AddMinutes(expiration);

            var jwt = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                expires: tokenExpiration,
                //notBefore:DateTime.Now,//AccessTokenExpiration zamanı şimdiden önce ise token geçerli değil.
                claims: SetClaims(user, operationClaims),
                signingCredentials: signingCredentials
            );

            return jwt;
        }

        private IEnumerable<Claim> SetClaims(User user, List<Claim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id);
            claims.AddPhoneNumber(user.PhoneNumber);
            claims.AddName($"{user.UserName} {user.NormalizedUserName}");
            //claims.AddName($"{user.FirstName} {user.LastName}");

            if (!string.IsNullOrEmpty(user.Email))
                claims.AddEmail(user.Email);

            var ip = operationClaims.Where(x => x.Type == ClaimTypes.Locality).Select(c => c.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(ip))
            {
                var ipClaim = operationClaims.Where(x => x.Type == ClaimTypes.Locality).Select(c => c.Value)
                     .FirstOrDefault();

                if (ipClaim != null)
                    claims.AddIp(ipClaim);
            }

            claims.AddRoles(operationClaims.Where(x => x.Type == ClaimTypes.Role).Select(c => c.Value).ToArray());
            return claims;
        }
    }
}

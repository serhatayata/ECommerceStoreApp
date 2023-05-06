using IdentityModel;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Extensions.Authentication;
using IdentityServer.Api.Utilities.Results;
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
        private IConfiguration _configuration { get; }
        private readonly JwtTokenOptions _tokenOptions;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _tokenOptions = _configuration.GetSection("JwtTokenOptions:VerifyCode").Get<JwtTokenOptions>();
        }

        public JwtAccessToken CreateToken(User user, List<Claim> operationClaims, int expiration,bool containsRefreshToken)
        {
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

        public DataResult<List<Claim>> ValidateCurrentToken(string token, string scheme)
        {
            var tokenOptions = _configuration.GetSection($"JwtTokenOptions:{scheme}").Get<JwtTokenOptions>();
            var tokenClaims = new List<Claim>();

            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.SecretKey));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenResult = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audience,
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);

                tokenClaims = tokenResult.Claims.ToList();
            }
            catch
            {
                return new ErrorDataResult<List<Claim>>();
            }

            return new SuccessDataResult<List<Claim>>(tokenClaims);
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
            claims.AddName($"{user.Name} {user.Surname}");

            if (!string.IsNullOrEmpty(user.Email))
                claims.AddEmail(user.Email);

            var ip = operationClaims.Where(x => x.Type == JwtClaimTypes.Locale).Select(c => c.Value).FirstOrDefault();
            if (!string.IsNullOrEmpty(ip))
            {
                var ipClaim = operationClaims.Where(x => x.Type == JwtClaimTypes.Locale).Select(c => c.Value)
                     .FirstOrDefault();

                if (ipClaim != null)
                    claims.AddIp(ipClaim);
            }

            claims.AddRoles(operationClaims.Where(x => x.Type == JwtClaimTypes.Role).Select(c => c.Value).ToArray());
            return claims;
        }
    }
}

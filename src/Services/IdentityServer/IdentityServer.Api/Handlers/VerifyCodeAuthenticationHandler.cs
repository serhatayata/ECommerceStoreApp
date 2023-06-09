using IdentityServer.Api.Extensions.Authentication;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Utilities.Security.Jwt;
using IdentityServer.Api.Utilities.Security.Jwt.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace IdentityServer.Api.Handlers
{
    public class VerifyCodeAuthenticationHandler : AuthenticationHandler<AppAuthenticationSchemeOptions>
    {
        private readonly IJwtHelper _jwtHelper;
        private readonly IConfiguration _configuration;

        private LoginOptions _loginOptions;

        public VerifyCodeAuthenticationHandler(IOptionsMonitor<AppAuthenticationSchemeOptions> options, 
                                               ILoggerFactory logger, 
                                               UrlEncoder encoder, 
                                               ISystemClock clock,
                                               IJwtHelper jwtHelper,
                                               IConfiguration configuration) : base(options, logger, encoder, clock)
        {
            _jwtHelper = jwtHelper;
            _configuration = configuration;

            _loginOptions = _configuration.GetValue<LoginOptions>("LoginOptions");
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            JwtTokenModel model;

            if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            {
                return Task.FromResult(AuthenticateResult.Fail("Header Not Found."));
            }

            var header = Request.Headers[HeaderNames.Authorization].ToString();
            var tokenMatch = Regex.Match(header, AuthenticationSchemeConstants.VerifyCode);

            if (tokenMatch.Success)
            {
                var token = header.Substring(AuthenticationSchemeConstants.VerifyCode.Length).TrimStart();

                try
                {
                    var tokenVerify = _jwtHelper.ValidateCurrentToken(token, AuthenticationSchemeConstants.VerifyCode);
                    if (!tokenVerify.Success)
                        return Task.FromResult(AuthenticateResult.Fail("TokenParseException"));

                    Claim[] claims = new Claim[1];
                    var claimsIdentity = new ClaimsIdentity(claims, AuthenticationSchemeConstants.VerifyCode);
                    var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), this.Scheme.Name);

                    string verifyRole = _loginOptions.VerifyCodeRole;
                    var tokenClaims = tokenVerify.Data;
                    var role = tokenClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                    if (role != null && role.Value == verifyRole)
                        return Task.FromResult(AuthenticateResult.Success(ticket));
                    else
                        return Task.FromResult(AuthenticateResult.Fail("TokenParseException"));
                }
                catch (Exception)
                {
                    //Log
                    return Task.FromResult(AuthenticateResult.Fail("TokenParseException"));
                }
            }

            // failure branch
            // return failure
            // with an optional message
            return Task.FromResult(AuthenticateResult.Fail("Token model is Empty"));
        }
    }
}

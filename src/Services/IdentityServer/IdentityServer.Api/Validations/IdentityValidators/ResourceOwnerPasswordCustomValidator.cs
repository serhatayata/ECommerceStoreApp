using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Extensions.Authentication;
using IdentityServer.Api.Models.UserModels;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Utilities.Security.Jwt;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using static IdentityModel.OidcConstants;

namespace IdentityServer.Api.Validations.IdentityValidators
{
    public class ResourceOwnerPasswordCustomValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEventService _events;
        private readonly IConfiguration _configuration;
        private readonly IRedisCacheService _redisCacheService;
        private readonly IJwtHelper _jwtHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly LoginOptions _loginOptions;

        public ResourceOwnerPasswordCustomValidator(UserManager<User> userManager,
                                                    SignInManager<User> signInManager,
                                                    IEventService events,
                                                    IConfiguration configuration,
                                                    IRedisCacheService redisCacheService,
                                                    IJwtHelper jwtHelper,
                                                    IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
            _configuration = configuration;
            _redisCacheService = redisCacheService;
            _jwtHelper = jwtHelper;
            _httpContextAccessor = httpContextAccessor;

            _loginOptions = _configuration.GetSection("LoginOptions").Get<LoginOptions>();
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var clientId = context.Request?.Client?.ClientId;
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null || string.IsNullOrWhiteSpace(request?.Headers.Authorization))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                return;
            }

            var authorizationHeader = request.Headers.Authorization.ToString();
            var token = authorizationHeader.Substring(AuthenticationSchemeConstants.VerifyCode.Length).TrimStart();

            string verifyRole = _loginOptions.VerifyCodeRole;

            var tokenVerify = _jwtHelper.ValidateCurrentToken(token, AuthenticationSchemeConstants.VerifyCode);
            var tokenClaims = tokenVerify.Data;
            var role = tokenClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (!tokenVerify.Success || role == null || role?.Value != verifyRole)
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", false, clientId));

                context.Result = new GrantValidationResult(TokenRequestErrors.UnauthorizedClient);
                return;
            }

            var verifyCode = context.Request?.Raw.Get("verifyCode");
            if (string.IsNullOrWhiteSpace(verifyCode))
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid verify code", false, clientId));

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                var existingCode = await _redisCacheService.GetAsyncWithDatabaseId<string>($"{_loginOptions.Prefix}{user.UserName}", _loginOptions.DatabaseId);
                if (existingCode == null || existingCode != verifyCode)
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "verify code not matched", false, clientId));

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
                    return;
                }

                var userId = await _userManager.GetUserIdAsync(user);
                var roles = await _userManager.GetRolesAsync(user);

                await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, userId, context.UserName, false, clientId));

                var claims = ClaimExtensions.GetUserClaims(user, roles.ToList());
                context.Result = new GrantValidationResult(subject: userId,
                                                           authenticationMethod: AuthenticationMethods.Password,
                                                           claims);
                return;
            }
            else
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid user", false, clientId));
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}

using IdentityModel;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Extensions;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Validations.ValidationContexts;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
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

        public ResourceOwnerPasswordCustomValidator(UserManager<User> userManager, 
                                                    SignInManager<User> signInManager, 
                                                    IEventService events,
                                                    IConfiguration configuration,
                                                    IRedisCacheService redisCacheService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
            _configuration = configuration;
            _redisCacheService = redisCacheService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var clientId = context.Request?.Client?.ClientId;

            var verifyCode = context.Request?.Raw.Get("verifyCode");
            if (string.IsNullOrWhiteSpace(verifyCode))
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "code not found", false, clientId));

            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                var result = await _signInManager.TwoFactorSignInAsync("Email", verifyCode, false, false);

                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var roles = await _userManager.GetRolesAsync(user);

                    await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, userId, context.UserName, false, clientId));

                    var claims = ClaimExtensions.GetUserClaims(user, roles.ToList());
                    context.Result = new GrantValidationResult(subject: userId, 
                                                               authenticationMethod: AuthenticationMethods.Password, 
                                                               claims);
                    return;
                }
                else if (result.IsLockedOut)
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "locked out", false, clientId));
                }
                else if (result.IsNotAllowed)
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", false, clientId));
                }
                else
                {
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid verify code", false, clientId));
                }
            }
            else
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username or password", false, clientId));
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}

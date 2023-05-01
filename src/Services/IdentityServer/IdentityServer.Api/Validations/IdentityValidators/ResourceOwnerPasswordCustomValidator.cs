using IdentityServer.Api.Entities.Identity;
using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using static IdentityModel.OidcConstants;

namespace IdentityServer.Api.Validations.IdentityValidators
{
    public class ResourceOwnerPasswordCustomValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEventService _events;

        public ResourceOwnerPasswordCustomValidator(UserManager<User> userManager, 
                                                    SignInManager<User> signInManager, 
                                                    IEventService events)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var clientId = context.Request?.Client?.ClientId;
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user: user, 
                                                                           password: context.Password, 
                                                                           lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    var sub = await _userManager.GetUserIdAsync(user);

                    await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, false, clientId));

                    context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
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
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", false, clientId));
                }
            }
            else
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", false, clientId));
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }
    }
}

using IdentityModel;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Services.Abstract;
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
        private readonly IRedisCacheService _redisCacheService;
        private readonly IConfiguration _configuration;

        public ResourceOwnerPasswordCustomValidator(UserManager<User> userManager, 
                                                    SignInManager<User> signInManager, 
                                                    IEventService events,
                                                    IRedisCacheService redisCacheService,
                                                    IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _events = events;
            _redisCacheService = redisCacheService;
            _configuration = configuration;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var clientId = context.Request?.Client?.ClientId;
            var user = await _userManager.FindByNameAsync(context.UserName);
            if (user != null)
            {
                string login2FAPrefix = _configuration.GetSection("LoginOptions:Prefix").Value;
                string login2FACode = await _redisCacheService.GetAsync<string>(login2FAPrefix + user.UserName);
                var result = await _signInManager.TwoFactorSignInAsync("Email", login2FACode, false, false);

                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    var roles = await _userManager.GetRolesAsync(user);

                    await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, userId, context.UserName, false, clientId));

                    var claims = GetUserClaims(user, roles.ToList());
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
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", false, clientId));
                }
            }
            else
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username or password", false, clientId));
            }

            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
        }

        public static Claim[] GetUserClaims(User user, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, user.Id),
                new Claim(JwtClaimTypes.Name, (!string.IsNullOrEmpty(user.Name) && !string.IsNullOrEmpty(user.Surname)) ? (user.Name + " " + user.Surname) : string.Empty),
                new Claim(JwtClaimTypes.GivenName, user.Name  ?? string.Empty),
                new Claim(JwtClaimTypes.FamilyName, user.Surname  ?? string.Empty),
                new Claim(JwtClaimTypes.Email, user.Email  ?? string.Empty),
            };

            foreach (var role in roles)
                claims.Add(new Claim(JwtClaimTypes.Role, role));

            return claims.ToArray();
        }
    }
}

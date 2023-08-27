using AutoMapper;
using IdentityModel;
using IdentityServer.Api.Data.Contexts;
using IdentityServer.Api.Entities.Identity;
using IdentityServer.Api.Extensions;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Api.Validations.IdentityValidators
{
    public class ProfileService : IProfileService
    {
        //services
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<ProfileService> _logger;

        public ProfileService(
            UserManager<User> userManager,
            IMapper mapper,
            ILogger<ProfileService> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        //Get user profile date in terms of claims when calling /connect/userinfo
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                if (!string.IsNullOrEmpty(context.Subject.Identity?.Name))
                {
                    //get user from db (in my case this is by email)
                    var user = await _userManager.FindByNameAsync(context.Subject.Identity.Name);

                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        var claims = ClaimExtensions.GetUserClaims(user, roles.ToList());

                        //set issued claims to return
                        context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                    }
                }
                else
                {
                    //get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id);

                    if (!string.IsNullOrEmpty(userId?.Value))
                    {
                        var user = await _userManager.FindByIdAsync(userId.Value);

                        // issue the claims for the user
                        if (user != null)
                        {
                            var roles = await _userManager.GetRolesAsync(user);
                            var claims = ClaimExtensions.GetUserClaims(user, roles.ToList());

                            //context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                            context.IssuedClaims = claims.ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("GetProfileDataAsync");
            }
        }

        //check if user account is active.
        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                //get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.Id);

                if (!string.IsNullOrEmpty(userId?.Value))
                {
                    var user = await _userManager.FindByIdAsync(userId.Value);

                    if (user != null)
                    {
                        if (user.Status == (byte)UserStatus.Validated)
                            context.IsActive = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical("IsActiveAsync");
            }
        }
    }
}

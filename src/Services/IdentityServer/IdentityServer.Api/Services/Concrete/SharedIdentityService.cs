using IdentityModel;
using IdentityServer.Api.Services.Abstract;
using System.Security.Claims;

namespace IdentityServer.Api.Services.Concrete
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _httpContextAccessor;

        public SharedIdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId => _httpContextAccessor.HttpContext.User.FindFirstValue(JwtClaimTypes.Id);
        public ClaimsPrincipal GetUser => _httpContextAccessor.HttpContext.User;
    }
}

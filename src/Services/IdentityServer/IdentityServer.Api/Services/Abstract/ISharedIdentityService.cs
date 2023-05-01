using System.Security.Claims;

namespace IdentityServer.Api.Services.Abstract
{
    public interface ISharedIdentityService
    {
        public string GetUserId { get; }
        public ClaimsPrincipal GetUser { get; }
    }
}

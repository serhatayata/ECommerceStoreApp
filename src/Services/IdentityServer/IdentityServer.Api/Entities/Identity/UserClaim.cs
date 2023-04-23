using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Api.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public DateTime CreateTime { get; set; }
    }
}

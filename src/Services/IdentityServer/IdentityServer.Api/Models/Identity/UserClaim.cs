using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Api.Models.Identity
{
    public class UserClaim : IdentityUserClaim<string>
    {
        public DateTime CreateTime { get; set; }
    }
}

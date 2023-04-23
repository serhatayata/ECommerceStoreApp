using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Api.Models.Identity
{
    public class UserToken:IdentityUserToken<string>
    {
        public DateTime CreateTime { get; set; }
    }
}

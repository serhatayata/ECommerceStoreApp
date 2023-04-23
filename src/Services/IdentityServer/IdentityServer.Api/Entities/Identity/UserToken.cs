using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Api.Entities.Identity
{
    public class UserToken : IdentityUserToken<string>
    {
        public DateTime CreateTime { get; set; }
    }
}

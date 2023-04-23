using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Api.Entities.Identity
{
    public class Role : IdentityRole<string>
    {
        public Role()
        {

        }

        public Role(string roleName) : this()
        {
            Name = roleName;
        }
    }
}

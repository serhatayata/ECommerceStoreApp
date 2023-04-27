using IdentityServer4.EntityFramework.Entities;

namespace IdentityServer.Api.Models.IncludeOptions.Account
{
    public class ApiResourceIncludeOptions : IBaseIncludeOptions
    {
        public bool Secrets { get; set; } = true;
        public bool Scopes { get; set; } = true;
        public bool UserClaims { get; set; } = true;
        public bool Properties { get; set; } = true;
    }
}

namespace IdentityServer.Api.Models.IncludeOptions.User
{
    public class UserIncludeOptions : IBaseIncludeOptions
    {
        public bool UserClaims { get; set; } = true;
        public bool UserLogins { get; set; } = true;
        public bool UserRoles { get; set; } = true;
        public bool UserTokens { get; set; } = true;
        public bool Roles { get; set; } = true;
        public bool RoleClaims { get; set; } = true;
    }
}

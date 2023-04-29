namespace IdentityServer.Api.Models.IncludeOptions.Account
{
    public class IdentityResourceIncludeOptions : IBaseIncludeOptions
    {
        public bool UserClaims { get; set; } = true;
        public bool Properties { get; set; } = true;
    }
}

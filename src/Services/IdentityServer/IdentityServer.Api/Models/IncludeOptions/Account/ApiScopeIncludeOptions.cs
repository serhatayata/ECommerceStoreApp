namespace IdentityServer.Api.Models.IncludeOptions.Account
{
    public class ApiScopeIncludeOptions : IBaseIncludeOptions
    {
        public bool UserClaims { get; set; } = true;
        public bool Properties { get; set; } = true;
    }
}

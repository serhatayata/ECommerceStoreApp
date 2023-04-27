namespace IdentityServer.Api.Models.IncludeOptions.Account
{
    public class ClientIncludeOptions : IBaseIncludeOptions
    {
        public bool Claims { get; set; } = true;
        public bool AllowedCorsOrigins { get; set; } = true;
        public bool IdentityProviderRestrictions { get; set; } = true;
        public bool PostLogoutRedirectUris { get; set; } = true;
        public bool Properties { get; set; } = true;
        public bool RedirectUris { get; set; } = true;
        public bool Scopes { get; set; } = true;
        public bool Secrets { get; set; } = true;
        public bool GrantTypes { get; set; } = true;
    }
}

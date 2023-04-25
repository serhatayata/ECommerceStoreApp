namespace IdentityServer.Api.Dtos
{
    public class ClientDto
    {
        /// <summary>
        /// Client id
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Client name to describe client
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// Secret keys for the client
        /// </summary>
        public List<IdentityServer4.EntityFramework.Entities.Secret> Secrets { get; set; }
        /// <summary>
        /// Allowed grant types
        /// </summary>
        public List<string> AllowedGrantTypes { get; set; }
        /// <summary>
        /// Where to redirect to after login
        /// </summary>
        public List<string> RedirectUris { get; set; }
        /// <summary>
        /// Where to redirect to after logout
        /// </summary>
        public List<string> PostLogoutRedirectUris { get; set; }
        /// <summary>
        /// Allowed CORS origins for JS clients
        /// </summary>
        public List<string> AllowedCorsOrigins { get; set; }
        /// <summary>
        /// Whether the token contains Refresh token or not
        /// </summary>
        public bool AllowOfflineAccess { get; set; }
        /// <summary>
        /// Allowed scopes for the client
        /// </summary>
        public List<string> AllowedScopes { get; set; }

    }
}

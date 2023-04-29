using IdentityServer.Api.Dtos.Base.Abstract;
using Newtonsoft.Json;

namespace IdentityServer.Api.Dtos.ClientDtos
{
    public class ClientDto : IDto
    {
        /// <summary>
        /// Client id
        /// </summary>
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
        /// <summary>
        /// Client name to describe client
        /// </summary>
        [JsonProperty("clientName")]
        public string ClientName { get; set; }
        /// <summary>
        /// Secret keys for the client
        /// </summary>
        [JsonProperty("secrets")]
        public List<IdentityServer4.Models.Secret> Secrets { get; set; }
        /// <summary>
        /// Allowed grant types
        /// </summary>
        [JsonProperty("allowedGrantTypes")]
        public List<string> AllowedGrantTypes { get; set; }
        /// <summary>
        /// Where to redirect to after login
        /// </summary>
        [JsonProperty("redirectUris")]
        public List<string> RedirectUris { get; set; }
        /// <summary>
        /// Where to redirect to after logout
        /// </summary>
        [JsonProperty("postLogoutRedirectUris")]
        public List<string> PostLogoutRedirectUris { get; set; }
        /// <summary>
        /// Allowed CORS origins for JS clients
        /// </summary>
        [JsonProperty("allowedCorsOrigins")]
        public List<string> AllowedCorsOrigins { get; set; }
        /// <summary>
        /// Whether the token contains Refresh token or not
        /// </summary>
        [JsonProperty("allowOfflineAccess")]
        public bool AllowOfflineAccess { get; set; }
        /// <summary>
        /// Allowed scopes for the client
        /// </summary>
        [JsonProperty("allowedScopes")]
        public List<string> AllowedScopes { get; set; }
        /// <summary>
        /// Properties for the client
        /// </summary>
        [JsonProperty("properties")]
        public List<IdentityServer4.EntityFramework.Entities.ClientProperty> Properties { get; set; }
    }
}

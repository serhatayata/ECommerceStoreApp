using IdentityServer.Api.Dtos.Base.Abstract;
using IdentityServer.Api.Dtos.Base.Concrete;
using Newtonsoft.Json;

namespace IdentityServer.Api.Dtos.ApiResourceDtos
{
    public class ApiResourceDto : IDto
    {
        /// <summary>
        /// enabled or not for api resource
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// Name of api resource
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Display name of api resource
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        /// <summary>
        /// Description of api resource
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// Allowed access token signing algorithms for api resource
        /// </summary>
        [JsonProperty("allowedAccessTokenSigningAlgorithms")]
        public string AllowedAccessTokenSigningAlgorithms { get; set; }
        /// <summary>
        /// Show in discovery document for api resource
        /// </summary>
        [JsonProperty("showInDiscoveryDocument")]
        public bool ShowInDiscoveryDocument { get; set; } = true;
        /// <summary>
        /// Secrets for api resource
        /// </summary>
        [JsonProperty("secrets")]
        public List<IdentityServer4.Models.Secret> Secrets { get; set; }
        /// <summary>
        /// Scopes of api resource
        /// </summary>
        [JsonProperty("scopes")]
        public List<string> Scopes { get; set; }
        /// <summary>
        /// UserClaims for api resource
        /// </summary>
        [JsonProperty("userClaims")]
        public List<string> UserClaims { get; set; }
        /// <summary>
        /// Properties for api resource
        /// </summary>
        [JsonProperty("properties")]
        public List<PropertyDto> Properties { get; set; }
        /// <summary>
        /// Non editable or not for api resource
        /// </summary>
        [JsonProperty("nonEditable")]
        public bool NonEditable { get; set; }
    }
}

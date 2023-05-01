using IdentityServer.Api.Models.Base.Abstract;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer4.EntityFramework.Entities;
using Newtonsoft.Json;

namespace IdentityServer.Api.Models.ApiScopeModels
{
    public class ApiScopeModel : IModel
    {
        /// <summary>
        /// enabled or not for api scope
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// name of api scope
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// display name of api scope
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        /// <summary>
        /// description of api scope
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// required or not for api scope
        /// </summary>
        [JsonProperty("required")]
        public bool Required { get; set; }
        /// <summary>
        /// Emphasize for api scope
        /// </summary>
        [JsonProperty("emphasize")]
        public bool Emphasize { get; set; }
        /// <summary>
        /// Show in discovery document or not for api scope
        /// </summary>
        [JsonProperty("showInDiscoveryDocument")]
        public bool ShowInDiscoveryDocument { get; set; } = true;
        /// <summary>
        /// User claims of api scope
        /// </summary>
        [JsonProperty("userClaims")]
        public List<string> UserClaims { get; set; }
        /// <summary>
        /// Properties of api scope
        /// </summary>
        [JsonProperty("properties")]
        public List<PropertyModel> Properties { get; set; }
    }
}

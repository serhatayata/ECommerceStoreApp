﻿using IdentityServer.Api.Models.Base.Abstract;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer4.EntityFramework.Entities;
using Newtonsoft.Json;

namespace IdentityServer.Api.Models.IdentityResourceModels
{
    public class IdentityResourceModel : IModel
    {
        /// <summary>
        /// enabled or not for identity resource
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// Identity resource name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// Identity resource display name
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
        /// <summary>
        /// Identity resource description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// Identity resource required or not
        /// </summary>
        [JsonProperty("required")]
        public bool Required { get; set; }
        /// <summary>
        /// Identity resource emphasize or not
        /// </summary>
        [JsonProperty("emphasize")]
        public bool Emphasize { get; set; }
        /// <summary>
        /// Identity resource show in discovery document or not
        /// </summary>
        [JsonProperty("showInDiscoveryDocument")]
        public bool ShowInDiscoveryDocument { get; set; } = true;
        /// <summary>
        /// Identity resource user claims
        /// </summary>
        [JsonProperty("userClaims")]
        public List<string> UserClaims { get; set; }
        /// <summary>
        /// Identity resource properties
        /// </summary>
        [JsonProperty("properties")]
        public List<PropertyModel> Properties { get; set; }
        /// <summary>
        /// Identity resource non editable or not
        /// </summary>
        [JsonProperty("nonEditable")]
        public bool NonEditable { get; set; }
    }
}

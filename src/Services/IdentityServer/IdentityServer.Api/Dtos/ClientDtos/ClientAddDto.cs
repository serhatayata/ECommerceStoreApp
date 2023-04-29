using IdentityServer.Api.Dtos.Base.Abstract;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using Newtonsoft.Json;

namespace IdentityServer.Api.Dtos.ClientDtos
{
    public class ClientAddDto : IAddDto
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
        /// <summary>
        /// Enabled or not for the client
        /// </summary>
        [JsonProperty("enabled")]
        public bool Enabled { get; set; } = true;
        /// <summary>
        /// Protocol type for the client
        /// </summary>
        [JsonProperty("protocolType")]
        public string ProtocolType { get; set; } = "oidc";
        /// <summary>
        /// Client secret is required or not
        /// </summary>
        [JsonProperty("requireClientSecret")]
        public bool RequireClientSecret { get; set; } = true;
        /// <summary>
        /// Client secret is required or not
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }
        /// <summary>
        /// Client uri for the client
        /// </summary>
        [JsonProperty("clientUri")]
        public string ClientUri { get; set; }
        /// <summary>
        /// Logout uri for the client
        /// </summary>
        [JsonProperty("logoutUri")]
        public string LogoUri { get; set; }
        /// <summary>
        /// Require consent or not for the client, default false
        /// </summary>
        [JsonProperty("requireConsent")]
        public bool RequireConsent { get; set; } = false;
        /// <summary>
        /// allow remember consent for the client, default true
        /// </summary>
        [JsonProperty("allowRememberConsent")]
        public bool AllowRememberConsent { get; set; } = true;
        /// <summary>
        /// Always include user claims in id token
        /// </summary>
        [JsonProperty("alwaysIncludeUserClaimsInIdToken")]
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        /// <summary>
        /// Require proof key for code exchange
        /// </summary>
        [JsonProperty("requirePkce")]
        public bool RequirePkce { get; set; } = true;
        /// <summary>
        /// Allow plain text proof key for exchange
        /// </summary>
        [JsonProperty("allowPlainTextPkce")]
        public bool AllowPlainTextPkce { get; set; }
        /// <summary>
        /// Require request object for the client
        /// </summary>
        [JsonProperty("requireRequestObject")]
        public bool RequireRequestObject { get; set; }
        /// <summary>
        /// Allow access tokens via browser for the client
        /// </summary>
        [JsonProperty("allowAccessTokensViaBrowser")]
        public bool AllowAccessTokensViaBrowser { get; set; }
        /// <summary>
        /// Front channel logout uri for the client
        /// </summary>
        [JsonProperty("frontChannelLogoutUri")]
        public string FrontChannelLogoutUri { get; set; }
        /// <summary>
        /// Front channel logout session required for the client, default true
        /// </summary>
        [JsonProperty("frontChannelLogoutSessionRequired")]
        public bool FrontChannelLogoutSessionRequired { get; set; } = true;
        /// <summary>
        /// Back channel logout logout uri for the client
        /// </summary>
        [JsonProperty("backChannelLogoutUri")]
        public string BackChannelLogoutUri { get; set; }
        /// <summary>
        /// Back channel logout session required for the client, default true
        /// </summary>
        [JsonProperty("backChannelLogoutSessionRequired")]
        public bool BackChannelLogoutSessionRequired { get; set; } = true;
        /// <summary>
        /// Identity token lifetime for the client, default 300
        /// </summary>
        [JsonProperty("identityTokenLifetime")]
        public int IdentityTokenLifetime { get; set; } = 300;
        /// <summary>
        /// Allowed identity token signing algorithms for the client
        /// </summary>
        [JsonProperty("allowedIdentityTokenSigningAlgorithms")]
        public string AllowedIdentityTokenSigningAlgorithms { get; set; }
        /// <summary>
        /// Access token lifetime for the client, default 3600
        /// </summary>
        [JsonProperty("accessTokenLifetime")]
        public int AccessTokenLifetime { get; set; } = 3600;
        /// <summary>
        /// Authorization code lifetime for the client, default 300
        /// </summary>
        [JsonProperty("authorizationCodeLifetime")]
        public int AuthorizationCodeLifetime { get; set; } = 300;
        /// <summary>
        /// Consent lifetime for the client, default null
        /// </summary>
        [JsonProperty("consentLifetime")]
        public int? ConsentLifetime { get; set; } = null;
        /// <summary>
        /// Absolute refresh token lifetime for the client, default 2592000
        /// </summary>
        [JsonProperty("absoluteRefreshTokenLifetime")]
        public int AbsoluteRefreshTokenLifetime { get; set; } = 2592000;
        /// <summary>
        /// Sliding refresh token lifetime for the client, default 1296000
        /// </summary>
        [JsonProperty("slidingRefreshTokenLifetime")]
        public int SlidingRefreshTokenLifetime { get; set; } = 1296000;
        /// <summary>
        /// Refresh token usage for the client, default <see cref="TokenExpiration.Absolute"/>
        /// </summary>
        [JsonProperty("refreshTokenUsage")]
        public int RefreshTokenUsage { get; set; } = (int)TokenUsage.OneTimeOnly;
        /// <summary>
        /// Update access token claims on refresh for the client
        /// </summary>
        [JsonProperty("updateAccessTokenClaimsOnRefresh")]
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        /// <summary>
        /// Refresh token expiration for the client, default <see cref="TokenExpiration.Absolute"/>
        /// </summary>
        [JsonProperty("refreshTokenExpiration")]
        public int RefreshTokenExpiration { get; set; } = (int)TokenExpiration.Absolute;
        /// <summary>
        /// Access token type for the client, Default 0 and it is AccessTokenType.Jwt
        /// </summary>
        [JsonProperty("accessTokenType")]
        public int AccessTokenType { get; set; } = (int)0; // AccessTokenType.Jwt;
        /// <summary>
        /// Enable local login for the client, default true
        /// </summary>
        [JsonProperty("enableLocalLogin")]
        public bool EnableLocalLogin { get; set; } = true;
        /// <summary>
        /// Client id provider restrictions for the client
        /// </summary>
        [JsonProperty("clientIdPRestriction")]
        public List<ClientIdPRestriction> IdentityProviderRestrictions { get; set; }
        /// <summary>
        /// Include jwt id for the client
        /// </summary>
        [JsonProperty("includeJwtId")]
        public bool IncludeJwtId { get; set; }
        /// <summary>
        /// Always send client claims for the client
        /// </summary>
        [JsonProperty("alwaysSendClientClaims")]
        public bool AlwaysSendClientClaims { get; set; }
        /// <summary>
        /// Client claims prefix for the client, default client_
        /// </summary>
        [JsonProperty("clientClaimsPrefix")]
        public string ClientClaimsPrefix { get; set; } = "client_";
        /// <summary>
        /// Pair wise subject salt for the client
        /// </summary>
        [JsonProperty("pairWiseSubjectSalt")]
        public string PairWiseSubjectSalt { get; set; }
        /// <summary>
        /// User sso lifetime for the client
        /// </summary>
        [JsonProperty("userSsoLifetime")]
        public int? UserSsoLifetime { get; set; }
        /// <summary>
        /// User code type for the client
        /// </summary>
        [JsonProperty("userCodeType")]
        public string UserCodeType { get; set; }
        /// <summary>
        /// Device code lifetime for the client, default 300
        /// </summary>
        [JsonProperty("deviceCodeLifetime")]
        public int DeviceCodeLifetime { get; set; } = 300;
        /// <summary>
        /// Non editable for the client
        /// </summary>
        [JsonProperty("nonEditable")]
        public bool NonEditable { get; set; }

    }
}

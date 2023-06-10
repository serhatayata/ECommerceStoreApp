using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using IdentityServer.Api.Models.Settings;
using IdentityServer.Api.Services.Token.Abstract;
using IdentityServer.Api.Utilities.Enums;
using IdentityServer.Api.Utilities.Results;
using IdentityServer.Api.Utilities.Security.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Extensions;

namespace IdentityServer.Api.Services.Token.Concrete
{
    public class ClientCredentialsTokenService : IClientCredentialsTokenService
    {
        private readonly SourceOrigin _sourceOrigin;
        private readonly IClientAccessTokenCache _clientAccessTokenCache;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IJwtHelper _jwtHelper;
        private readonly ILogger<ClientCredentialsTokenService> _logger;

        public ClientCredentialsTokenService(IOptions<SourceOrigin> sourceOrigin,
                            IClientAccessTokenCache clientAccessTokenCache,
                            HttpClient httpClient,
                            ILogger<ClientCredentialsTokenService> logger,
                            IJwtHelper jwtHelper,
                            IConfiguration configuration)
        {
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            _jwtHelper = jwtHelper;

            _sourceOrigin = sourceOrigin.Value;
        }

        /// <summary>
        /// This method creates only token for itself
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<DataResult<string>> GetToken(EnumProjectType type)
        {
            var typeName = type.GetDisplayName();
            var currentToken = await _clientAccessTokenCache.GetAsync($"{typeName}-token", new ClientAccessTokenParameters());

            if (currentToken != null)
                return new SuccessDataResult<string>(currentToken.AccessToken);

            var clientId = _configuration.GetSection($"ClientSettings:{typeName}:FullPermission:ClientId").Value;
            var clientSecret = _configuration.GetSection($"ClientSettings:{typeName}:FullPermission:ClientSecret").Value;
            var duration = _configuration.GetSection($"ClientSettings:{typeName}:FullPermission:Duration").Get<int>();

            var jwtIssuer = _configuration.GetSection("Jwt:Issuer").Value;
            var jwtAudience = _configuration.GetSection("Jwt:Audience").Value;

            var newToken = _jwtHelper.CreateApiToken(new Utilities.Security.Jwt.Models.JwtApiTokenOptions()
            {
                ClientId = clientId,
                SecretKey = clientSecret,
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                AccessTokenExpiration = duration
            }, duration);
            //var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

            if (newToken == null)
                throw new Exception($"{nameof(this.GetToken)} -- ClientId : {clientId} api token request error");

            await _clientAccessTokenCache.SetAsync($"{typeName}-token", 
                                                     newToken.AccessToken,
                                                     duration, 
                                                     new ClientAccessTokenParameters());

            return new SuccessDataResult<string>(newToken.AccessToken);
        }
    }
}

using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using IdentityServer.Api.Models.Settings;
using IdentityServer.Api.Services.Token.Abstract;
using IdentityServer.Api.Utilities.Enums;
using IdentityServer.Api.Utilities.Results;
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
        private readonly ILogger<ClientCredentialsTokenService> _logger;

        public ClientCredentialsTokenService(IOptions<SourceOrigin> sourceOrigin,
                            IClientAccessTokenCache clientAccessTokenCache,
                            HttpClient httpClient,
                            ILogger<ClientCredentialsTokenService> logger,
                            IConfiguration configuration)
        {
            _clientAccessTokenCache = clientAccessTokenCache;
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            _sourceOrigin = sourceOrigin.Value;
        }

        public async Task<DataResult<string>> GetToken(EnumProjectType type)
        {
            var typeName = type.GetDisplayName();
            var currentToken = await _clientAccessTokenCache.GetAsync($"{typeName}-token", new ClientAccessTokenParameters());

            if (currentToken != null)
                return new SuccessDataResult<string>(currentToken.AccessToken);

            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _sourceOrigin.Identity,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (disco.IsError)
                throw disco.Exception;

            var clientId = _configuration.GetSection($"ClientSettings:{typeName}:FullPermission:ClientId").Value;
            var clientSecret = _configuration.GetSection($"ClientSettings:{typeName}:FullPermission:ClientSecret").Value;

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Address = disco.TokenEndpoint
            };

            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);

            if (newToken.IsError)
                throw newToken.Exception;

            await _clientAccessTokenCache.SetAsync($"{typeName}-token", 
                                                     newToken.AccessToken, 
                                                     newToken.ExpiresIn, 
                                                     new ClientAccessTokenParameters());

            return new SuccessDataResult<string>(newToken.AccessToken);
        }
    }
}

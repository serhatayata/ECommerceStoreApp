using BasketService.Api.Models;
using BasketService.Api.Services.Token.Abstract;
using BasketService.Api.Utilities.Enums;
using BasketService.Api.Utilities.Results;
using IdentityModel.Client;
using Microsoft.OpenApi.Extensions;
using System.Net.Http;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Net.Http.Headers;

namespace BasketService.Api.Services.Token.Concrete
{
    public class ClientCredentialsTokenService : IClientCredentialsTokenService
    {
        static readonly string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public ClientCredentialsTokenService(IConfiguration configuration,
                                             HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        /// <summary>
        /// This method gets only token for localization server from IdentityServer
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<DataResult<string>> GetToken(EnumProjectType type, ApiPermissionType permissionType)
        {
            var typeName = type.GetDisplayName();
            var permissionTypeName = permissionType.GetDisplayName();
            var localizationSettings = _configuration.GetSection($"ClientSettings:{typeName}:{permissionTypeName}").Get<PermissionModel>();

            var identityBaseUri = _configuration.GetSection($"IdentityServerConfigurations:Url").Value;

            var disco = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = identityBaseUri,
                Policy = new DiscoveryPolicy { 
                    RequireHttps = false, 
                    ValidateIssuerName = false,
                    ValidateEndpoints = false
                }
            });

            if (disco.IsError)
                throw disco.Exception;

            var clientCredentialTokenRequest = new ClientCredentialsTokenRequest
            {
                ClientId = localizationSettings.ClientId,
                ClientSecret = localizationSettings.ClientSecret,
                Address = disco.TokenEndpoint
            };

            var newToken = await _httpClient.RequestClientCredentialsTokenAsync(clientCredentialTokenRequest);
            if (newToken.IsError)
                throw newToken.Exception;

            return new SuccessDataResult<string>(newToken.AccessToken);
        }
    }
}

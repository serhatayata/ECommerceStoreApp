using IdentityServer.Api.CacheStores.Models;
using IdentityServer.Api.Models.Base.Concrete;
using IdentityServer.Api.Models.ClientModels;
using IdentityServer.Api.Models.IncludeOptions.Account;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Services.Redis.Abstract;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer.Api.CacheStores
{
    public class CustomClientStore : IClientStore
    {
        private readonly IClientService _clientService;
        private readonly ILogger<CustomClientStore> _logger;
        private readonly IRedisService _redisService;
        private readonly IConfiguration _configuration;

        private CustomStoreConfiguration _customStoreConfiguration;

        public CustomClientStore(IClientService clientService, 
                                 ILogger<CustomClientStore> logger, 
                                 IRedisService redisService, 
                                 IConfiguration configuration)
        {
            _clientService = clientService;
            _logger = logger;
            _redisService = redisService;
            _configuration = configuration;

            _customStoreConfiguration = _configuration.GetSection("CustomStoreConfigurations:CustomClientStore")
                                                      .Get<CustomStoreConfiguration>();
        }

        public async Task<IdentityServer4.Models.Client> FindClientByIdAsync(string clientId)
        {
            var prefix = _customStoreConfiguration.Prefix;
            var duration = _customStoreConfiguration.Duration;

            var cacheValue = await _redisService.GetAsync<ClientModel>(prefix + clientId);
            if (cacheValue != null)
                return new IdentityServer4.Models.Client()
                {
                    ClientId = cacheValue.ClientId,
                    AllowedGrantTypes = cacheValue.AllowedGrantTypes,
                    AllowedScopes = cacheValue.AllowedScopes,
                    ClientSecrets = cacheValue.Secrets
                };

            var client = _clientService.Get(new StringModel() { Value = clientId },
                                            new ClientIncludeOptions());

            if (client == null)
                return null;

            await _redisService.SetAsync(prefix + clientId, client.Data, duration);

            ICollection<string> type = null;
            switch (client.Data.AllowedGrantTypes.FirstOrDefault())
            {
                case GrantType.ResourceOwnerPassword:
                    type = GrantTypes.ResourceOwnerPassword;
                    break;
                case GrantType.ClientCredentials:
                    type = GrantTypes.ClientCredentials;
                    break;
                default:
                    type = GrantTypes.ResourceOwnerPassword;
                    break;
            }

            return new IdentityServer4.Models.Client()
            {
                ClientId = client.Data.ClientId,
                AllowedGrantTypes = type,
                AllowedScopes = client.Data.AllowedScopes,
                ClientSecrets = client.Data.Secrets
            };
        }
    }
}

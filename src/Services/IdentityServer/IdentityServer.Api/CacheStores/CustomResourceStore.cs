using AutoMapper;
using IdentityServer.Api.CacheStores.Models;
using IdentityServer.Api.Services.Abstract;
using IdentityServer.Api.Services.Redis.Abstract;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace IdentityServer.Api.CacheStores
{
    public class CustomResourceStore : IResourceStore
    {
        private readonly IApiResourceService _apiResourceService;
        private readonly IApiScopeService _apiScopeService;
        private readonly IIdentityResourceService _identityResourceService;
        private readonly ILogger<CustomClientStore> _logger;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        private readonly CustomStoreConfiguration _customStoreConfiguration;
        private readonly string _prefix;
        private readonly int _duration;

        public CustomResourceStore(IApiResourceService apiResourceService, 
                                   IApiScopeService apiScopeService, 
                                   IIdentityResourceService identityResourceService, 
                                   ILogger<CustomClientStore> logger, 
                                   IRedisService redisService,
                                   IMapper mapper,
                                   IConfiguration configuration)
        {
            _apiResourceService = apiResourceService;
            _apiScopeService = apiScopeService;
            _identityResourceService = identityResourceService;
            _logger = logger;
            _redisService = redisService;
            _mapper = mapper;
            _configuration = configuration;

            _customStoreConfiguration = _configuration.GetSection("CustomStoreConfigurations:CustomResourceStore")
                                                      .Get<CustomStoreConfiguration>();

            _prefix = _customStoreConfiguration.Prefix;
            _duration = _customStoreConfiguration.Duration;
        }

        public async Task<IEnumerable<IdentityServer4.Models.ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            var apiResources = _apiResourceService.Get(apiResourceNames.ToList(), new());
            var mappedResources = _mapper.Map<List<IdentityServer4.Models.ApiResource>>(apiResources.Data);

            return await Task.FromResult(mappedResources);
        }

        public async Task<IEnumerable<IdentityServer4.Models.ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var apiResources = _apiResourceService.GetByApiScopeNames(scopeNames.ToList(), new());
            var mappedResources = _mapper.Map<List<IdentityServer4.Models.ApiResource>>(apiResources.Data);

            return await Task.FromResult(mappedResources);
        }

        public async Task<IEnumerable<IdentityServer4.Models.ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            var apiScopes = _apiScopeService.Get(scopeNames.ToList(), new());
            var mappedResources = _mapper.Map<List<IdentityServer4.Models.ApiScope>>(apiScopes.Data);

            return await Task.FromResult(mappedResources);
        }

        public async Task<IEnumerable<IdentityServer4.Models.IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            var identityResources = _identityResourceService.Get(scopeNames.ToList(), new());
            var mappedResources = _mapper.Map<List<IdentityServer4.Models.IdentityResource>>(identityResources.Data);

            return await Task.FromResult(mappedResources);
        }

        public async Task<IdentityServer4.Models.Resources> GetAllResourcesAsync()
        {
            IdentityServer4.Models.Resources resources = new();

            var identityResources = _identityResourceService.GetAll(new());
            var apiScopes = _apiScopeService.GetAll(new());
            var apiResources = _apiResourceService.GetAll(new());

            resources.IdentityResources = _mapper.Map<List<IdentityServer4.Models.IdentityResource>>(identityResources.Data);
            resources.ApiScopes = _mapper.Map<List<IdentityServer4.Models.ApiScope>>(apiScopes.Data);
            resources.ApiResources = _mapper.Map<List<IdentityServer4.Models.ApiResource>>(apiResources.Data);

            resources.OfflineAccess = false;

            return await Task.FromResult(resources);
        }
    }
}

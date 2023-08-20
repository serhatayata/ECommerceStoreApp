using AutoMapper;
using CatalogService.Api.IntegrationEvents.Events;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Services.Elastic.Abstract;
using EventBus.Base.Abstraction;

namespace CatalogService.Api.IntegrationEvents.EventHandling
{
    public class ProductUpdatedIntegrationEventHandler : IIntegrationEventHandler<ProductUpdatedIntegrationEvent>
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly ILogger<ProductUpdatedIntegrationEventHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public ProductUpdatedIntegrationEventHandler(
            IElasticSearchService elasticSearchService, 
            ILogger<ProductUpdatedIntegrationEventHandler> logger, 
            IMapper mapper,
            IConfiguration configuration)
        {
            _elasticSearchService = elasticSearchService;
            _logger = logger;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task Handle(ProductUpdatedIntegrationEvent @event)
        {
            try
            {
                var searchIndex = _configuration.GetSection("ElasticSearchIndex:Product:Search").Value;

                var productElasticModel = _mapper.Map<ProductElasticModel>(@event);
                var result = await _elasticSearchService.CreateOrUpdateAsync<ProductElasticModel>(searchIndex, productElasticModel);

                if (!result)
                    throw new Exception($"{nameof(ProductUpdatedIntegrationEvent)} : Update error");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
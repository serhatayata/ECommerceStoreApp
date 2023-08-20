using AutoMapper;
using CatalogService.Api.IntegrationEvents.Events;
using CatalogService.Api.Services.Elastic.Abstract;
using EventBus.Base.Abstraction;

namespace CatalogService.Api.IntegrationEvents.EventHandling
{
    public class ProductUpdatedIntegrationEventHandler : IIntegrationEventHandler<ProductUpdatedIntegrationEvent>
    {
        private readonly IElasticSearchService _elasticSearchService;
        private readonly ILogger<ProductUpdatedIntegrationEventHandler> _logger;
        private readonly IMapper _mapper;

        public ProductUpdatedIntegrationEventHandler(
            IElasticSearchService elasticSearchService, 
            ILogger<ProductUpdatedIntegrationEventHandler> logger, 
            IMapper mapper)
        {
            _elasticSearchService = elasticSearchService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Handle(ProductUpdatedIntegrationEvent @event)
        {
            // Consumer received kısmında try catchde eğer catche düşerse acknowledge okey gitmesin, eğer acknowledge giderse kuyruktan düşüyormu ona bak,
            // Burada acknowledge işlemi, update doğru şekilde yapılırsa yapılsın, eğer update yapılmazsa kuyruktan düşmesin daha sonra aktif olunca yapılır.


        }
    }
}
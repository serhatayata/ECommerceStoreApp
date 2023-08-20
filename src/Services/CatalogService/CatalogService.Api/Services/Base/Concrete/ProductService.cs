using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Extensions;
using CatalogService.Api.IntegrationEvents;
using CatalogService.Api.IntegrationEvents.Events;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Services.Elastic.Abstract;
using CatalogService.Api.Utilities.Results;
using EventBus.Base.Abstraction;
using Polly;
using System.Reflection;

namespace CatalogService.Api.Services.Base.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IEfProductRepository _efProductRepository;
        private readonly IDapperProductRepository _dapperProductRepository;
        private readonly IElasticSearchService _elasticSearchService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ICatalogIntegrationEventService _catalogIntegrationEventService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<ProductService> _logger;

        private readonly string productSearchIndex;
        private readonly int codeLength;

        public ProductService(
            IEfProductRepository efProductRepository, 
            IDapperProductRepository dapperProductRepository, 
            IElasticSearchService elasticSearchService,
            IMapper mapper,
            IConfiguration configuration,
            ICatalogIntegrationEventService catalogIntegrationEventService,
            ILogger<ProductService> logger,
            IEventBus eventBus)
        {
            _efProductRepository = efProductRepository;
            _dapperProductRepository = dapperProductRepository;
            _elasticSearchService = elasticSearchService;
            _mapper = mapper;
            _configuration = configuration;
            _catalogIntegrationEventService = catalogIntegrationEventService;
            _logger = logger;
            _eventBus = eventBus;

            codeLength = _configuration.GetValue<int>("ProductCodeGenerationLength");
            productSearchIndex = _configuration.GetSection("ElasticSearchIndex:Product:Search").Value ?? string.Empty;
        }

        public async Task<Result> AddAsync(ProductAddModel entity)
        {
            var mappedModel = _mapper.Map<Product>(entity);
            // Code generation
            var code = DataGenerationExtensions.RandomCode(codeLength);
            //Code exists
            var productCodeExists = await _efProductRepository.GetAsync(c => c.ProductCode == code);
            if (productCodeExists.Data != null)
                return new ErrorResult("Product code already exists");

            mappedModel.ProductCode = code;
            mappedModel.UpdateDate = DateTime.Now;
            mappedModel.Link = this.GetProductLink(DataGenerationExtensions.GenerateLinkData(entity.Name), code);

            return await _efProductRepository.AddAsync(mappedModel);
        }

        public async Task<Result> UpdateAsync(ProductUpdateModel entity)
        {
            var mappedModel = _mapper.Map<Product>(entity);
            //Check if name changed
            var existingProduct = await _dapperProductRepository.GetAsync(new IntModel(entity.Id));
            if (existingProduct?.Data == null)
                return new ErrorResult("Product not found");

            if (entity.Name != existingProduct.Data.Name)
            {
                var code = DataGenerationExtensions.RandomCode(codeLength);
                mappedModel.ProductCode = code;
                mappedModel.Link = this.GetProductLink(DataGenerationExtensions.GenerateLinkData(entity.Name), code);
            }
            else
            {
                mappedModel.ProductCode = existingProduct.Data.ProductCode;
                mappedModel.Link = existingProduct.Data.Link;
            }

            mappedModel.UpdateDate = DateTime.Now;
            var result = await _efProductRepository.UpdateAsync(mappedModel);

            if (result.Success && entity.Price != existingProduct.Data.Price)
            {
                //Creating integration event to be published
                var priceChangedEvent = new ProductPriceChangedIntegrationEvent(existingProduct.Data.Id,
                                                                                entity.Price,
                                                                                existingProduct.Data.Price);

                await _catalogIntegrationEventService.SaveEventAndCatalogContextChangesAsync(priceChangedEvent);

                // Publish and mark the saved event as published
                await _catalogIntegrationEventService.PublishThroughEventBusAsync(priceChangedEvent);
            }

            if (result.Success)
                this.SendProductUpdateEvent(entity);

            return result;
        }

        public async Task<Result> DeleteAsync(IntModel model)
        {
            return await _efProductRepository.DeleteAsync(model);
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetAllAsync()
        {
            var result = await _dapperProductRepository.GetAllAsync();
            return _mapper.Map<DataResult<IReadOnlyList<ProductModel>>>(result);
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetAllBetweenPricesAsync(PriceBetweenModel model)
        {
            var result = await _dapperProductRepository.GetAllBetweenPricesAsync(model);
            return _mapper.Map<DataResult<IReadOnlyList<ProductModel>>>(result);
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetAllByBrandIdAsync(IntModel model)
        {
            var result = await _dapperProductRepository.GetAllByBrandIdAsync(model);
            return _mapper.Map<DataResult<IReadOnlyList<ProductModel>>>(result);
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetAllByProductTypeIdAsync(IntModel model)
        {
            var result = await _dapperProductRepository.GetAllByProductTypeIdAsync(model);
            return _mapper.Map<DataResult<IReadOnlyList<ProductModel>>>(result);
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetAllPagedAsync(PagingModel model)
        {
            var result = await _dapperProductRepository.GetAllPagedAsync(model);
            return _mapper.Map<DataResult<IReadOnlyList<ProductModel>>>(result);
        }

        public async Task<DataResult<ProductModel>> GetAsync(IntModel model)
        {
            var result = await _dapperProductRepository.GetAsync(model);
            return _mapper.Map<DataResult<ProductModel>>(result);
        }

        public async Task<DataResult<ProductModel>> GetByProductCodeAsync(StringModel model)
        {
            var result = await _dapperProductRepository.GetByProductCodeAsync(model);
            return _mapper.Map<DataResult<ProductModel>>(result);
        }

        

        public async Task<DataResult<ProductSearchModel>> SearchAsync(string name, bool includeSuggest = false, bool aggs = false)
        {
            var index = this.productSearchIndex;

            var searchResponse = await _elasticSearchService
                .GetClient()
                .SearchAsync<ProductElasticModel>(s => s
                .Index(index)
                .Query(query => query.
                    Bool(bo => bo.
                        Must(mu => mu.
                            Match(mat => mat.
                                Field(f => f.Name)))))
                .Aggregations(ags =>
                {
                    if (aggs)
                    {
                        return ags
                         .Min("min_price", m => m.Field(f => f.Price))
                         .Max("max_price", m => m.Field(f => f.Price))
                         .Sum("total_available_stock", s => s.Field(f => f.AvailableStock))
                         .Min("min_available_stock", s => s.Field(f => f.AvailableStock))
                         .Max("max_available_stock", s => s.Field(f => f.AvailableStock));
                    }

                    return null;
                })
            );

            var suggests = new List<ProductSuggest>();

            if (includeSuggest)
            {
                var suggestResponse = await _elasticSearchService
                    .GetClient()
                    .SearchAsync<ProductElasticModel>(s => s
                    .Index(index)
                    .Suggest(s => s.Completion("suggestions", c => c
                                      .Field(f => f.NameSuggest)
                                      .Prefix(name)
                                      .Size(5)
                                      .SkipDuplicates()
                                      .Fuzzy(f => f
                                        .Fuzziness(Nest.Fuzziness.EditDistance(1))
                                        .MinLength(4)
                                      ))));



                if (suggestResponse?.Suggest.ContainsKey("suggestions") ?? false)
                    suggests = suggestResponse.Suggest["suggestions"]
                                             .SelectMany(s => s.Options)
                                             .Select(option => new ProductSuggest()
                                             {
                                                 Id = option.Source.Id,
                                                 Name = option.Source.Name,
                                                 SuggestedName = option.Text,
                                                 Score = option.Score
                                             })
                                             .OrderByDescending(o => o.Score)
                                             .ToList();
            }

            ProductSearchModel result = new();

            var elasticProducts = searchResponse?.Documents;
            var products = _mapper.Map<List<ProductModel>>(elasticProducts);

            result.Products = products;
            result.Suggests = suggests;

            return new DataResult<ProductSearchModel>(result);
        }

        public async Task<DataResult<IEnumerable<ProductSuggest>?>> SearchSuggestAsync(string index, string name)
        {
            var searchResponse = await _elasticSearchService
                .GetClient()
                .SearchAsync<ProductElasticModel>(s => s
                .Index(index)
                .Suggest(s => s
                   .Completion("suggestions", c => c
                      .Field(f => f.NameSuggest)
                      .Prefix(name)
                      .Size(5)
                      .SkipDuplicates()
                      .Fuzzy(f => f
                         .Fuzziness(Nest.Fuzziness.EditDistance(2))
                         .MinLength(4)
                      )
                   )
                )
            );

            var suggests = new List<ProductSuggest>();

            if (searchResponse?.Suggest.ContainsKey("suggestions") ?? false)
                suggests = searchResponse.Suggest["suggestions"]
                                         .SelectMany(s => s.Options)
                                         .Select(option => new ProductSuggest()
                                         {
                                             Id = option.Source.Id,
                                             Name = option.Source.Name,
                                             SuggestedName = option.Text,
                                             Score = option.Score
                                         })
                                         .OrderByDescending(o => o.Score)
                                         .ToList();

            return new DataResult<IEnumerable<ProductSuggest>?>(suggests ?? new List<ProductSuggest>());
        }

        public async Task<Result> CreateElasticIndex(string index)
        {
            var client = _elasticSearchService.GetClient();

            var createResponse = await client.Indices.CreateAsync(index,
                cr => cr.
                   Settings(sett => sett
                     .Analysis(anlys => anlys
                        .TokenFilters(tf => tf
                           .Synonym("product_synonym", st =>
                               st.Synonyms(
                                  "hizli, guclu",
                                  "vantilator, pervane"
                               )
                           )
                           .NGram("nGram_filter", ng =>
                              ng.MinGram(2)
                                .MaxGram(10)
                           )
                        )
                        .Analyzers(anly => anly
                            .Custom("custom_standard", cs =>
                               cs.Tokenizer("standard")
                                 .Filters(
                                    "lowercase",
                                    "asciifolding"
                                  ))
                            .Custom("nGram_analyzer", cs =>
                               cs.Tokenizer("whitespace")
                                 .Filters(
                                    "lowercase",
                                    "asciifolding",
                                    "nGram_filter"
                                 )
                            )
                       )
                     )
                     .Setting(Nest.UpdatableIndexSettings.MaxNGramDiff, 11)
                   )
                .Map<ProductElasticModel>(m => m
                    .AutoMap()
                    .Properties(p => p
                        .Number(n => n
                            .Name(na => na.Id)
                            .Type(Nest.NumberType.Integer)
                        )
                        .Text(t => t
                            .Name(n => n.Name)
                            .Analyzer("nGram_analyzer")
                        )
                        .Text(d => d
                            .Name(na => na.Description)
                            .Analyzer("custom_standard")
                        )
                        .Number(d => d
                            .Name(na => na.Price)
                            .Type(Nest.NumberType.Double)
                        )
                        .Number(n => n
                            .Name(n => n.AvailableStock)
                            .Type(Nest.NumberType.Integer)
                        )
                        .Text(l => l
                            .Name(n => n.Link)
                            .Analyzer("custom_standard")
                        )
                        .Text(p => p
                            .Name(n => n.ProductCode)
                            .Analyzer("custom_standard")
                        )
                        .Number(n => n
                            .Name(n => n.ProductTypeId)
                            .Type(Nest.NumberType.Integer)
                        )
                        .Number(n => n
                            .Name(n => n.BrandId)
                            .Type(Nest.NumberType.Integer)
                        )
                        .Date(d => d
                            .Name(n => n.CreateDate)
                        )
                        .Date(d => d
                            .Name(n => n.UpdateDate)
                        )
                        .Completion(c => c
                            .Name(comp => comp.NameSuggest)
                            .Analyzer("simple")
                            .SearchAnalyzer("simple")
                            .MaxInputLength(20)
                            .PreservePositionIncrements()
                            .PreserveSeparators()
                        )
                    )
                )
            );

            return new SuccessResult();
        }

        private string GetProductLink(string linkData, string code) => string.Join("-", linkData, code);
        
        private void SendProductUpdateEvent(ProductUpdateModel model)
        {
            var currentEvent = _mapper.Map<ProductUpdatedIntegrationEvent>(model);
            _eventBus.Publish(currentEvent);
        }
    }
}

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

        private int _codeLength;

        public ProductService(
            IEfProductRepository efProductRepository, 
            IDapperProductRepository dapperProductRepository, 
            IElasticSearchService elasticSearchService,
            IMapper mapper, 
            IConfiguration configuration,
            ICatalogIntegrationEventService catalogIntegrationEventService)
        {
            _efProductRepository = efProductRepository;
            _dapperProductRepository = dapperProductRepository;
            _elasticSearchService = elasticSearchService;
            _mapper = mapper;
            _configuration = configuration;
            _catalogIntegrationEventService = catalogIntegrationEventService;

            _codeLength = _configuration.GetValue<int>("ProductCodeGenerationLength");
        }

        public async Task<Result> AddAsync(ProductAddModel entity)
        {
            var mappedModel = _mapper.Map<Product>(entity);
            // Code generation
            var code = DataGenerationExtensions.RandomCode(_codeLength);
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
                var code = DataGenerationExtensions.RandomCode(_codeLength);
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

        private string GetProductLink(string linkData, string code) => string.Join("-", linkData, code);


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
                .Map<Product>(m => m
                    .Properties(p => p
                        .Number(n => n
                            .Name(na => na.Id)
                        )
                        .Text(t => t
                            .Name(n => n.Name)
                            .Analyzer("nGram_analyzer")
                        )
                        .Text(d => d
                            .Name(na => na.Description)
                            .Analyzer("custom_standard")
                        )
                        .DoubleRange(d => d
                            .Name(na => na.Price)
                        )
                        .Number(n => n
                            .Name(n => n.AvailableStock)
                        )
                        .Text(l => l
                            .Name(n => n.Link)
                            .Analyzer("custom_standard")
                        )
                        .Text(p => p
                            .Name(n => n.ProductCode)
                            .Analyzer("custom_standard")
                        )
                        .Date(d => d
                            .Name(n => n.CreateDate)
                        )
                        .Date(d => d
                            .Name(n => n.UpdateDate)
                        )
                    )
                )
            );

            return new SuccessResult();
        }
    }
}

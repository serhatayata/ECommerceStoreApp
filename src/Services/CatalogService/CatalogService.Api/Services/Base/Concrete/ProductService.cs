using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IEfProductRepository _efProductRepository;
        private readonly IDapperProductRepository _dapperProductRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private int _codeLength;

        public ProductService(
            IEfProductRepository efProductRepository, 
            IDapperProductRepository dapperProductRepository, 
            IMapper mapper, 
            IConfiguration configuration)
        {
            _efProductRepository = efProductRepository;
            _dapperProductRepository = dapperProductRepository;
            _mapper = mapper;
            _configuration = configuration;

            _codeLength = _configuration.GetValue<int>("ProductCodeGenerationLength");
        }

        public async Task<Result> AddAsync(ProductAddModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> UpdateAsync(ProductUpdateModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteAsync(IntModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetAllBetweenPricesAsync(PriceBetweenModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetAllByBrandIdAsync(IntModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetAllByProductTypeIdAsync(IntModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetAllPagedAsync(PagingModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<ProductModel>> GetAsync(IntModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<ProductModel>> GetByProductCodeAsync(StringModel model)
        {
            throw new NotImplementedException();
        }
    }
}

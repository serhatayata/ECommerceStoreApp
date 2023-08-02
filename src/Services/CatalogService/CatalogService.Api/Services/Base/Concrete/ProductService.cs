using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;
using CatalogService.Api.Entities;
using CatalogService.Api.Extensions;
using CatalogService.Api.Data.Repositories.EntityFramework.Concrete;
using CatalogService.Api.Data.Repositories.Dapper.Concrete;

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
            if (existingProduct?.Data != null && entity.Name != existingProduct.Data.Name)
            {
                var code = DataGenerationExtensions.RandomCode(_codeLength);
                mappedModel.ProductCode = code;
                mappedModel.Link = this.GetProductLink(DataGenerationExtensions.GenerateLinkData(entity.Name), code);
            }

            mappedModel.UpdateDate = DateTime.Now;
            return await _efProductRepository.UpdateAsync(mappedModel);
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
    }
}

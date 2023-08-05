using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Concrete
{
    public class FeatureService : IFeatureService
    {
        private readonly IEfFeatureRepository _efFeatureRepository;
        private readonly IDapperFeatureRepository _dapperFeatureRepository;
        private readonly IMapper _mapper;

        public FeatureService(
            IEfFeatureRepository efFeatureRepository, 
            IDapperFeatureRepository dapperFeatureRepository, 
            IMapper mapper)
        {
            _efFeatureRepository = efFeatureRepository;
            _dapperFeatureRepository = dapperFeatureRepository;
            _mapper = mapper;
        }

        public async Task<Result> AddAsync(FeatureAddModel entity)
        {
            var featureExists = await _efFeatureRepository.GetAsync(f => f.Name == entity.Name);
            if (featureExists.Success)
                return new ErrorResult("Feature name already exists");

            var mappedModel = _mapper.Map<Feature>(entity);
            var result = await _efFeatureRepository.AddAsync(mappedModel);
            return result;
        }

        public async Task<Result> UpdateAsync(FeatureUpdateModel entity)
        {
            var featureExists = await _efFeatureRepository.GetAsync(f => f.Id != entity.Id && f.Name == entity.Name);
            if (featureExists.Success)
                return new ErrorResult("Feature name already exists");

            var mappedModel = _mapper.Map<Feature>(entity);
            var result = await _efFeatureRepository.UpdateAsync(mappedModel);
            return result;
        }

        public async Task<Result> DeleteAsync(IntModel model)
        {
            return await _efFeatureRepository.DeleteAsync(model);
        }

        public async Task<Result> AddProductFeatureAsync(ProductFeatureModel entity)
        {
            var productFeatureExists = await _dapperFeatureRepository.GetProductFeature(entity);
            if (productFeatureExists.Data != null)
                return new ErrorResult("Product feature already exists");

            var mappedModel = _mapper.Map<ProductFeature>(entity);
            var result = await _efFeatureRepository.AddProductFeatureAsync(mappedModel);
            return result;
        }

        public async Task<Result> DeleteProductFeatureAsync(ProductFeatureModel entity)
        {
            return await _efFeatureRepository.DeleteProductFeatureAsync(entity);
        }

        public async Task<Result> AddProductFeaturePropertyAsync(ProductFeaturePropertyAddModel entity)
        {
            var propertyExists = await _dapperFeatureRepository.GetProductFeatureProperty(entity.ProductFeatureId, entity.Name);
            if (propertyExists.Data != null)
                return new ErrorResult("Property already exists for this product feature");

            var mappedModel = _mapper.Map<ProductFeatureProperty>(entity);
            var result = await _efFeatureRepository.AddProductFeaturePropertyAsync(mappedModel);
            return result;
        }

        public async Task<Result> UpdateProductFeaturePropertyAsync(ProductFeaturePropertyUpdateModel entity)
        {
            var propertyExists = await _dapperFeatureRepository.GetProductFeatureProperty(new IntModel(entity.Id));
            if (propertyExists.Data == null)
                return new ErrorResult("Property does not exist");

            var mappedModel = _mapper.Map<ProductFeatureProperty>(entity);
            var result = await _efFeatureRepository.UpdateProductFeaturePropertyAsync(mappedModel);
            return result;
        }

        public async Task<Result> DeleteProductFeaturePropertyAsync(IntModel entity)
        {
            return await _efFeatureRepository.DeleteProductFeaturePropertyAsync(entity);
        }

        public async Task<DataResult<FeatureModel>> GetAsync(IntModel model)
        {
            var result = await _dapperFeatureRepository.GetAsync(model);
            var resultData = _mapper.Map<DataResult<FeatureModel>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllAsync()
        {
            var result = await _dapperFeatureRepository.GetAllAsync();
            var resultData = _mapper.Map<DataResult<IReadOnlyList<FeatureModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<ProductFeaturePropertyModel>>> GetAllFeatureProperties(ProductFeatureModel model)
        {
            var result = await _dapperFeatureRepository.GetAllFeatureProperties(model.FeatureId, model.ProductId);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<ProductFeaturePropertyModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<ProductFeaturePropertyModel>>> GetAllFeaturePropertiesByProductFeatureId(IntModel model)
        {
            var result = await _dapperFeatureRepository.GetAllFeaturePropertiesByProductFeatureId(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<ProductFeaturePropertyModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllFeaturesByProductCode(StringModel model)
        {
            var result = await _dapperFeatureRepository.GetAllFeaturesByProductCode(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<FeatureModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllFeaturesByProductId(IntModel model)
        {
            var result = await _dapperFeatureRepository.GetAllFeaturesByProductId(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<FeatureModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllPagedAsync(PagingModel model)
        {
            var result = await _dapperFeatureRepository.GetAllPagedAsync(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<FeatureModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<ProductModel>>> GetFeatureProducts(IntModel model)
        {
            var result = await _dapperFeatureRepository.GetFeatureProducts(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<ProductModel>>>(result);
            return resultData;
        }
    }
}

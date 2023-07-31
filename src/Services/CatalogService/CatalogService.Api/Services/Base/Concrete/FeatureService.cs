using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.FeatureModels;
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
            var featureExists = _efFeatureRepository.GetAsync(f => f.Name == entity.Name);
            if (featureExists != null)
                return new ErrorResult("Feature name already exists");

            var mappedModel = _mapper.Map<Feature>(entity);
            var result = await _efFeatureRepository.AddAsync(mappedModel);
            return result;
        }

        public async Task<Result> UpdateAsync(FeatureUpdateModel entity)
        {
            var featureExists = await _efFeatureRepository.GetAsync(f => f.Id != entity.Id && f.Name == entity.Name);
            if (featureExists != null)
                return new ErrorResult("Feature name already exists");

            var mappedModel = _mapper.Map<Feature>(entity);
            var result = await _efFeatureRepository.UpdateAsync(mappedModel);
            return result;
        }

        public async Task<Result> AddProductFeatureAsync(ProductFeatureModel entity)
        {
            
        }

        public async Task<Result> DeleteAsync(IntModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> DeleteProductFeatureAsync(IntModel entity)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<FeatureModel>> GetAsync(IntModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<ProductFeaturePropertyModel>>> GetAllFeatureProperties(ProductFeatureModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<ProductFeaturePropertyModel>>> GetAllFeaturePropertiesByProductFeatureId(IntModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllFeaturesByProductCode(StringModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllFeaturesByProductId(IntModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<FeatureModel>>> GetAllPagedAsync(PagingModel model)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResult<IReadOnlyList<Product>>> GetFeatureProducts(IntModel model)
        {
            throw new NotImplementedException();
        }
    }
}

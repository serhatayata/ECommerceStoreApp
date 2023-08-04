using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.BrandModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Concrete
{
    public class BrandService : IBrandService
    {
        private readonly IEfBrandRepository _efBrandRepository;
        private readonly IDapperBrandRepository _dapperBrandRepository;
        private readonly IMapper _mapper;

        public BrandService(
            IEfBrandRepository efBrandRepository, 
            IDapperBrandRepository dapperBrandRepository, 
            IMapper mapper)
        {
            _efBrandRepository = efBrandRepository;
            _dapperBrandRepository = dapperBrandRepository;
            _mapper = mapper;
        }

        public async Task<Result> AddAsync(BrandAddModel model)
        {
            var mappedModel = _mapper.Map<Brand>(model);

            //Same name check
            var nameExists = _efBrandRepository.GetAsync(b => b.Name == model.Name);
            if (nameExists != null)
                return new ErrorResult("Brand name already exists");

            var result = await _efBrandRepository.AddAsync(mappedModel);
            return result;
        }

        public async Task<Result> UpdateAsync(BrandUpdateModel model)
        {
            var mappedModel = _mapper.Map<Brand>(model);
            //Same name check
            var nameExists = _efBrandRepository.GetAsync(b => b.Name == model.Name && b.Id != model.Id);
            if (nameExists != null)
                return new ErrorResult("Brand name already exists");

            var result = await _efBrandRepository.UpdateAsync(mappedModel);
            return result;
        }

        public async Task<Result> DeleteAsync(IntModel model)
        {
            var result = await _efBrandRepository.DeleteAsync(model);
            return result;
        }

        public async Task<DataResult<BrandModel>> GetAsync(IntModel model)
        {
            var result = await _dapperBrandRepository.GetAsync(model);
            var resultData = _mapper.Map<DataResult<BrandModel>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<BrandModel>>> GetAllAsync()
        {
            var result = await _dapperBrandRepository.GetAllAsync();
            var resultData = _mapper.Map<DataResult<IReadOnlyList<BrandModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<BrandModel>>> GetAllPagedAsync(PagingModel model)
        {
            var result = await _dapperBrandRepository.GetAllPagedAsync(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<BrandModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<BrandModel>>> GetAllWithProductsAsync()
        {
            var result = await _dapperBrandRepository.GetAllWithProductsAsync();
            var resultData = _mapper.Map<DataResult<IReadOnlyList<BrandModel>>>(result);
            return resultData;
        }
    }
}

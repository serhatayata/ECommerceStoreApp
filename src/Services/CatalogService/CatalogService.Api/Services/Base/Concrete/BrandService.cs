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
        private readonly ILogger<BrandService> _logger;

        public BrandService(
            IEfBrandRepository efBrandRepository, 
            IDapperBrandRepository dapperBrandRepository, 
            IMapper mapper, 
            ILogger<BrandService> logger)
        {
            _efBrandRepository = efBrandRepository;
            _dapperBrandRepository = dapperBrandRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> AddAsync(BrandAddModel model)
        {
            var mappedModel = _mapper.Map<Brand>(model);

            var result = await _efBrandRepository.AddAsync(mappedModel);
            return result;
        }

        public async Task<Result> UpdateAsync(BrandUpdateModel model)
        {
            var mappedModel = _mapper.Map<Brand>(model);

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
            var result = await _efBrandRepository.GetAsync(model);
            var resultData = _mapper.Map<DataResult<BrandModel>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<BrandModel>>> GetAllAsync()
        {
            var result = await _efBrandRepository.GetAllAsync();
            var resultData = _mapper.Map<DataResult<IReadOnlyList<BrandModel>>>(result);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<BrandModel>>> GetAllPagedAsync(PagingModel model)
        {
            var result = await _dapperBrandRepository.GetAllPagedAsync(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<BrandModel>>>(result.Data);
            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<BrandModel>>> GetAllWithProductsAsync()
        {
            var result = await _dapperBrandRepository.GetAllWithProductsAsync();
            var resultData = _mapper.Map<DataResult<IReadOnlyList<BrandModel>>>(result.Data);
            return resultData;
        }
    }
}

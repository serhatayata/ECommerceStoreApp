using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.BrandModels;
using CatalogService.Api.Models.CategoryModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IEfCategoryRepository _efBrandRepository;
        private readonly IDapperCategoryRepository _dapperBrandRepository;
        private readonly IMapper _mapper;

        public CategoryService(
            IEfCategoryRepository efBrandRepository, 
            IDapperCategoryRepository dapperBrandRepository, 
            IMapper mapper)
        {
            _efBrandRepository = efBrandRepository;
            _dapperBrandRepository = dapperBrandRepository;
            _mapper = mapper;
        }

        public async Task<Result> AddAsync(CategoryAddModel entity)
        {
            var mappedModel = _mapper.Map<Category>(entity);
            var result = await _efBrandRepository.AddAsync(mappedModel);

            return result;
        }

        public async Task<Result> UpdateAsync(CategoryUpdateModel entity)
        {
            var mappedModel = _mapper.Map<Category>(entity);
            var result = await _efBrandRepository.UpdateAsync(mappedModel);

            return result;
        }

        public async Task<Result> DeleteAsync(IntModel model)
        {
            var result = await _efBrandRepository.DeleteAsync(model);
            return result;
        }

        public async Task<DataResult<CategoryModel>> GetAsync(IntModel model)
        {
            var result = await _dapperBrandRepository.GetAsync(model);
            var resultData = _mapper.Map<DataResult<CategoryModel>>(result);

            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllAsync()
        {
            var result = await _dapperBrandRepository.GetAllAsync();
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CategoryModel>>>(result);

            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllByParentId(IntModel model)
        {
            var result = await _dapperBrandRepository.GetAllByParentId(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CategoryModel>>>(result);

            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllPagedAsync(PagingModel model)
        {
            var result = await _dapperBrandRepository.GetAllPagedAsync(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CategoryModel>>>(result);

            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllWithProductsByParentId(IntModel model)
        {
            var result = await _dapperBrandRepository.GetAllWithProductsByParentId(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CategoryModel>>>(result);

            return resultData;
        }

        public async Task<DataResult<CategoryModel>> GetByName(StringModel model)
        {
            var result = await _dapperBrandRepository.GetByName(model);
            var resultData = _mapper.Map<DataResult<CategoryModel>>(result);

            return resultData;
        }

        public async Task<DataResult<CategoryModel>> GetByNameWithProducts(StringModel model)
        {
            var result = await _dapperBrandRepository.GetByNameWithProducts(model);
            var resultData = _mapper.Map<DataResult<CategoryModel>>(result);

            return resultData;
        }

        public async Task<DataResult<CategoryModel>> GetWithProducts(IntModel model)
        {
            var result = await _dapperBrandRepository.GetWithProducts(model);
            var resultData = _mapper.Map<DataResult<CategoryModel>>(result);

            return resultData;
        }
    }
}

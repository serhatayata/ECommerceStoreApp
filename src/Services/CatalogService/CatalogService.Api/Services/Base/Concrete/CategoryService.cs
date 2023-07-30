using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Extensions;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.BrandModels;
using CatalogService.Api.Models.CategoryModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Services.Base.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IEfCategoryRepository _efCategoryRepository;
        private readonly IDapperCategoryRepository _dapperCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public CategoryService(
            IEfCategoryRepository efCategoryRepository, 
            IDapperCategoryRepository dapperCategoryRepository, 
            IMapper mapper,
            IConfiguration configuration)
        {
            _efCategoryRepository = efCategoryRepository;
            _dapperCategoryRepository = dapperCategoryRepository;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Result> AddAsync(CategoryAddModel entity)
        {
            var mappedModel = _mapper.Map<Category>(entity);
            //Code generation
            var codeLength = _configuration.GetValue<int>("CategoryCodeGenerationLength");
            var code = DataGenerationExtensions.RandomCode(codeLength);
            //Code exists
            var categoryCodeExists = await _efCategoryRepository.GetAsync(c => c.Code == code);
            if (categoryCodeExists.Data != null)
                return new ErrorResult("Category code already exists");

            mappedModel.Code = code;
            mappedModel.UpdateDate = DateTime.Now;
            mappedModel.Link = this.GetCategoryLink(DataGenerationExtensions.GenerateLinkData(entity.Name), code);

            var result = await _efCategoryRepository.AddAsync(mappedModel);
            return result;
        }

        public async Task<Result> UpdateAsync(CategoryUpdateModel entity)
        {
            var mappedModel = _mapper.Map<Category>(entity);
            var result = await _efCategoryRepository.UpdateAsync(mappedModel);

            return result;
        }

        public async Task<Result> DeleteAsync(IntModel model)
        {
            var result = await _efCategoryRepository.DeleteAsync(model);
            return result;
        }

        public async Task<DataResult<CategoryModel>> GetAsync(IntModel model)
        {
            var result = await _dapperCategoryRepository.GetAsync(model);
            var resultData = _mapper.Map<DataResult<CategoryModel>>(result);

            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllAsync()
        {
            var result = await _dapperCategoryRepository.GetAllAsync();
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CategoryModel>>>(result);

            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllByParentId(IntModel model)
        {
            var result = await _dapperCategoryRepository.GetAllByParentId(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CategoryModel>>>(result);

            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllPagedAsync(PagingModel model)
        {
            var result = await _dapperCategoryRepository.GetAllPagedAsync(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CategoryModel>>>(result);

            return resultData;
        }

        public async Task<DataResult<IReadOnlyList<CategoryModel>>> GetAllWithProductsByParentId(IntModel model)
        {
            var result = await _dapperCategoryRepository.GetAllWithProductsByParentId(model);
            var resultData = _mapper.Map<DataResult<IReadOnlyList<CategoryModel>>>(result);

            return resultData;
        }

        public async Task<DataResult<CategoryModel>> GetByName(StringModel model)
        {
            var result = await _dapperCategoryRepository.GetByName(model);
            var resultData = _mapper.Map<DataResult<CategoryModel>>(result);

            return resultData;
        }

        public async Task<DataResult<CategoryModel>> GetByNameWithProducts(StringModel model)
        {
            var result = await _dapperCategoryRepository.GetByNameWithProducts(model);
            var resultData = _mapper.Map<DataResult<CategoryModel>>(result);

            return resultData;
        }

        public async Task<DataResult<CategoryModel>> GetWithProducts(IntModel model)
        {
            var result = await _dapperCategoryRepository.GetWithProducts(model);
            var resultData = _mapper.Map<DataResult<CategoryModel>>(result);

            return resultData;
        }

        // PRIVATE METHODS

        private string GetCategoryLink(string linkData, string code) => string.Join("-", linkData, code);
    }
}

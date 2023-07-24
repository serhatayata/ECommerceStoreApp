using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Models.Base.Concrete;
using Grpc.Core;

namespace CatalogService.Api.Services.Grpc
{
    public class GrpcCategoryService : CategoryProtoService.CategoryProtoServiceBase
    {
        private readonly IDapperCategoryRepository _dapperCategoryRepository;
        private readonly IEfCategoryRepository _efCategoryRepository;
        private readonly IMapper _mapper;

        public GrpcCategoryService(IDapperCategoryRepository dapperCategoryRepository, IEfCategoryRepository efCategoryRepository, IMapper mapper)
        {
            _dapperCategoryRepository = dapperCategoryRepository;
            _efCategoryRepository = efCategoryRepository;
            _mapper = mapper;
        }

        public override async Task<GrpcCategoryModel> GetAsync(GrpcIntModel request, ServerCallContext context)
        {
            var model = _mapper.Map<IntModel>(request);

            var result = await _dapperCategoryRepository.GetAsync(model);
            var resultModel = _mapper.Map<GrpcCategoryModel>(result.Data);

            return resultModel;
        }

        public override async Task<GrpcCategoryModel> GetByNameAsync(GrpcStringModel request, ServerCallContext context)
        {
            var model = _mapper.Map<StringModel>(request);

            var result = await _dapperCategoryRepository.GetByName(model);
            var resultModel = _mapper.Map<GrpcCategoryModel>(result.Data);

            return resultModel;
        }

        public override async Task<GrpcCategory> GetByNameWithProductsAsync(GrpcStringModel request, ServerCallContext context)
        {
            var model = _mapper.Map<StringModel>(request);

            var result = await _dapperCategoryRepository.GetByNameWithProducts(model);
            var resultModel = _mapper.Map<GrpcCategory>(result.Data);

            return resultModel;
        }

        public override async Task<GrpcCategory> GetWithProductsAsync(GrpcIntModel request, ServerCallContext context)
        {
            var model = _mapper.Map<IntModel>(request);

            var result = await _dapperCategoryRepository.GetWithProducts(model);
            var resultModel = _mapper.Map<GrpcCategory>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcCategoryModel> GetAllAsync(GrpcEmptyModel request, ServerCallContext context)
        {
            var result = await _dapperCategoryRepository.GetAllAsync();
            var resultModel = _mapper.Map<ListGrpcCategoryModel>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcCategoryModel> GetAllByParentIdAsync(GrpcIntModel request, ServerCallContext context)
        {
            var model = _mapper.Map<IntModel>(request);

            var result = await _dapperCategoryRepository.GetAllByParentId(model);
            var resultModel = _mapper.Map<ListGrpcCategoryModel>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcCategory> GetAllWithProductsByParentId(GrpcIntModel request, ServerCallContext context)
        {
            var model = _mapper.Map<IntModel>(request);

            var result = await _dapperCategoryRepository.GetAllWithProductsByParentId(model);
            var resultModel = _mapper.Map<ListGrpcCategory>(result.Data);

            return resultModel;
        }
    }
}

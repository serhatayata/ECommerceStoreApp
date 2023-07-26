using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Concrete;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Services.Grpc.Abstract;
using Grpc.Core;

namespace CatalogService.Api.Services.Grpc
{
    public class GrpcFeatureService : BaseGrpcFeatureService
    {
        private readonly IDapperFeatureRepository _dapperFeatureRepository;
        private readonly IMapper _mapper;

        public GrpcFeatureService(
            IDapperFeatureRepository dapperFeatureRepository, 
            IMapper mapper)
        {
            _dapperFeatureRepository = dapperFeatureRepository;
            _mapper = mapper;
        }

        public override async Task<GrpcFeatureModel> GetAsync(GrpcIntModel request, ServerCallContext context)
        {
            var model = _mapper.Map<IntModel>(request);

            var result = await _dapperFeatureRepository.GetAsync(model);
            var resultModel = _mapper.Map<GrpcFeatureModel>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcFeatureModel> GetAllAsync(GrpcEmptyModel request, ServerCallContext context)
        {
            var result = await _dapperFeatureRepository.GetAllAsync();
            var resultModel = _mapper.Map<ListGrpcFeatureModel>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcFeature> GetAllFeaturesByProductId(GrpcIntModel request, ServerCallContext context)
        {
            var model = _mapper.Map<IntModel>(request);

            var result = await _dapperFeatureRepository.GetAllFeaturesByProductId(model);
            var resultModel = _mapper.Map<ListGrpcFeature>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcFeatureModel> GetAllFeaturesByProductCode(GrpcStringModel request, ServerCallContext context)
        {
            var model = _mapper.Map<StringModel>(request);

            var result = await _dapperFeatureRepository.GetAllFeaturesByProductCode(model);
            var resultModel = _mapper.Map<ListGrpcFeatureModel>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcProductModel> GetFeatureProducts(GrpcIntModel request, ServerCallContext context)
        {
            var model = _mapper.Map<IntModel>(request);

            var result = await _dapperFeatureRepository.GetFeatureProducts(model);
            var resultModel = _mapper.Map<ListGrpcProductModel>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcProductFeaturePropertyModel> GetAllFeatureProperties(GrpcProductFeatureModel request, ServerCallContext context)
        {
            var result = await _dapperFeatureRepository.GetAllFeatureProperties(request.FeatureId,request.ProductId);
            var resultModel = _mapper.Map<ListGrpcProductFeaturePropertyModel>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcProductFeaturePropertyModel> GetAllFeaturePropertiesByProductFeatureId(GrpcIntModel request, ServerCallContext context)
        {
            var model = _mapper.Map<IntModel>(request);

            var result = await _dapperFeatureRepository.GetAllFeaturePropertiesByProductFeatureId(model);
            var resultModel = _mapper.Map<ListGrpcProductFeaturePropertyModel>(result.Data);

            return resultModel;
        }
    }
}

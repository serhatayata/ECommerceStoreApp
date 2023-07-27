using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Concrete;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Services.Grpc.Abstract;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace CatalogService.Api.Services.Grpc
{
    public class GrpcFeatureService : BaseGrpcFeatureService
    {
        private readonly IDapperFeatureRepository _dapperFeatureRepository;
        private readonly IMapper _mapper;
        private readonly IRedisService _redisService;
        private RedisOptions _redisOptions;

        public GrpcFeatureService(
            IDapperFeatureRepository dapperFeatureRepository, 
            IMapper mapper,
            IRedisService redisService,
            IOptions<RedisOptions> redisOptions)
        {
            _dapperFeatureRepository = dapperFeatureRepository;
            _mapper = mapper;
            _redisService = redisService;
            _redisOptions = redisOptions.Value;
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
            var cacheKey = this.CurrentCacheKey(nameof(GetAllAsync));
            var result = await _redisService.GetAsync<ListGrpcFeatureModel>(
                cacheKey,
                _redisOptions.DatabaseId,
                _redisOptions.Duration,
                async () =>
                {
                    var result = await _dapperFeatureRepository.GetAllAsync();
                    var resultModel = _mapper.Map<ListGrpcFeatureModel>(result.Data);

                    return resultModel;
                });

            return result;
        }

        public override async Task<ListGrpcFeature> GetAllFeaturesByProductId(GrpcIntModel request, ServerCallContext context)
        {
            var cacheKey = this.CurrentCacheKey(methodName: nameof(GetAllFeaturesByProductId), parameters: request.Value.ToString());
            var result = await _redisService.GetAsync<ListGrpcFeature>(
                cacheKey,
                _redisOptions.DatabaseId,
                _redisOptions.Duration,
                async () =>
                {
                    var model = _mapper.Map<IntModel>(request);

                    var result = await _dapperFeatureRepository.GetAllFeaturesByProductId(model);
                    var resultModel = _mapper.Map<ListGrpcFeature>(result.Data);

                    return resultModel;
                });

            return result;
        }

        public override async Task<ListGrpcFeatureModel> GetAllFeaturesByProductCode(GrpcStringModel request, ServerCallContext context)
        {
            var cacheKey = this.CurrentCacheKey(methodName: nameof(GetAllFeaturesByProductCode), parameters: request.Value.ToString());
            var result = await _redisService.GetAsync<ListGrpcFeatureModel>(
                cacheKey,
                _redisOptions.DatabaseId,
                _redisOptions.Duration,
                async () =>
                {
                    var model = _mapper.Map<StringModel>(request);

                    var result = await _dapperFeatureRepository.GetAllFeaturesByProductCode(model);
                    var resultModel = _mapper.Map<ListGrpcFeatureModel>(result.Data);

                    return resultModel;
                });

            return result;
        }

        public override async Task<ListGrpcProductModel> GetFeatureProducts(GrpcIntModel request, ServerCallContext context)
        {
            var cacheKey = this.CurrentCacheKey(methodName: nameof(GetFeatureProducts), parameters: request.Value.ToString());
            var result = await _redisService.GetAsync<ListGrpcProductModel>(
                cacheKey,
                _redisOptions.DatabaseId,
                _redisOptions.Duration,
                async () =>
                {
                    var model = _mapper.Map<IntModel>(request);

                    var result = await _dapperFeatureRepository.GetFeatureProducts(model);
                    var resultModel = _mapper.Map<ListGrpcProductModel>(result.Data);

                    return resultModel;
                });

            return result;
        }

        public override async Task<ListGrpcProductFeaturePropertyModel> GetAllFeatureProperties(GrpcProductFeatureModel request, ServerCallContext context)
        {
            var cacheKey = this.CurrentCacheKey(methodName: nameof(GetAllFeatureProperties), 
                                                parameters: new string[] { request.FeatureId.ToString(), request.ProductId.ToString() });
            var result = await _redisService.GetAsync<ListGrpcProductFeaturePropertyModel>(
                cacheKey,
                _redisOptions.DatabaseId,
                _redisOptions.Duration,
                async () =>
                {
                    var result = await _dapperFeatureRepository.GetAllFeatureProperties(request.FeatureId, request.ProductId);
                    var resultModel = _mapper.Map<ListGrpcProductFeaturePropertyModel>(result.Data);

                    return resultModel;
                });

            return result;
        }

        public override async Task<ListGrpcProductFeaturePropertyModel> GetAllFeaturePropertiesByProductFeatureId(GrpcIntModel request, ServerCallContext context)
        {
            var cacheKey = this.CurrentCacheKey(methodName: nameof(GetAllFeaturePropertiesByProductFeatureId), parameters: request.Value.ToString());
            var result = await _redisService.GetAsync<ListGrpcProductFeaturePropertyModel>(
                cacheKey,
                _redisOptions.DatabaseId,
                _redisOptions.Duration,
                async () =>
                {
                    var model = _mapper.Map<IntModel>(request);

                    var result = await _dapperFeatureRepository.GetAllFeaturePropertiesByProductFeatureId(model);
                    var resultModel = _mapper.Map<ListGrpcProductFeaturePropertyModel>(result.Data);

                    return resultModel;
                });

            return result;
        }

        public override Task<ListGrpcFeatureModel> GetAllPagedAsync(GrpcPagingModel request, ServerCallContext context)
        {
            return base.GetAllPagedAsync(request, context);
        }
    }
}

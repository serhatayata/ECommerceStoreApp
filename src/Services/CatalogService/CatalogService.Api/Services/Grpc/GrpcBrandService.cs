using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Extensions;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Services.Grpc.Abstract;
using CatalogService.Api.Utilities.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace CatalogService.Api.Services.Grpc
{
    public class GrpcBrandService : BaseGrpcBrandService
    {
        private readonly IDapperBrandRepository _dapperBrandRepository;
        private readonly IRedisService _redisService;
        private readonly IMapper _mapper;
        private readonly RedisOptions _redisOptions;

        public GrpcBrandService(
            IDapperBrandRepository dapperBrandRepository,
            IRedisService redisService,
            IMapper mapper,
            IOptions<RedisOptions> redisOptions) 
        {
            _dapperBrandRepository = dapperBrandRepository;
            _redisService = redisService;
            _mapper = mapper;
            _redisOptions = redisOptions.Value;
        }

        public override async Task<GrpcBrandModel> GetAsync(GrpcIntModel request, ServerCallContext context)
        {
            var cacheKey = this.GetCacheKey(System.Reflection.MethodBase.GetCurrentMethod()?.Name ?? nameof(GetAsync));
            return _redisService.GetAsync<GrpcBrandModel>(
                cacheKey, 
                _redisOptions.DatabaseId, 
                _redisOptions.Duration,
                async () =>
                {
                    var requestModel = _mapper.Map<IntModel>(request);
                    var result = await _dapperBrandRepository.GetAsync(requestModel);

                    var resultData = _mapper.Map<GrpcBrandModel>(result.Data);
                    return resultData;
                });
        }

        public override async Task<ListGrpcBrandModel> GetAllAsync(GrpcEmptyModel request, ServerCallContext context)
        {
            var result = await _dapperBrandRepository.GetAllAsync();
            var resultData = _mapper.Map<ListGrpcBrandModel>(result.Data);

            return resultData;
        }

        public override async Task<ListGrpcBrand> GetAllWithProductsAsync(GrpcEmptyModel request, ServerCallContext context)
        {
            var result = await _dapperBrandRepository.GetAllWithProductsAsync();
            var resultData = _mapper.Map<ListGrpcBrand>(result.Data);

            return resultData;
        }
    }
}

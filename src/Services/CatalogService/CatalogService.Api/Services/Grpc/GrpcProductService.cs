using AutoMapper;
using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Concrete;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Services.Grpc.Abstract;
using Grpc.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CatalogService.Api.Services.Grpc;

public class GrpcProductService : BaseGrpcProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRedisService _redisService;
    private RedisOptions _redisOptions;

    public GrpcProductService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IRedisService redisService,
        IOptions<RedisOptions> redisOptions,
        IHttpContextAccessor httpContextAccessor)
        : base(httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _redisService = redisService;
        _redisOptions = redisOptions.Value;
    }

    public override async Task<GrpcProductModel> GetByProductCodeAsync(GrpcStringModel request, ServerCallContext context)
    {
        var requestData = _mapper.Map<StringModel>(request);

        var result = await _unitOfWork.DapperProductRepository.GetByProductCodeAsync(requestData);
        var resultData = _mapper.Map<GrpcProductModel>(result.Data);

        return resultData;
    }

    public override async Task<ListGrpcProductModel> GetAllAsync(GrpcEmptyModel request, ServerCallContext context)
    {
        var cacheKey = this.CurrentCacheKey(nameof(GetAllAsync));
        var result = await _redisService.GetAsync<ListGrpcProductModel>(
            cacheKey,
            _redisOptions.DatabaseId,
            _redisOptions.Duration,
        async () =>
        {
            var result = await _unitOfWork.DapperProductRepository.GetAllAsync();
            var resultData = _mapper.Map<ListGrpcProductModel>(result.Data);

            return resultData;
        });

        return result;
    }

    public override async Task<ListGrpcProductModel> GetAllBetweenPricesAsync(GrpcPriceBetweenModel request, ServerCallContext context)
    {
        var cacheKey = this.CurrentCacheKey(methodName: nameof(GetAllBetweenPricesAsync), 
                                            parameters: new string[] { request.MinimumPrice.ToString(), request.MaximumPrice.ToString() });
        var result = await _redisService.GetAsync<ListGrpcProductModel>(
            cacheKey,
            _redisOptions.DatabaseId,
            _redisOptions.Duration,
            async () =>
            {
                var minimumPrice = Convert.ToDecimal(request.MinimumPrice);
                var maximumPrice = Convert.ToDecimal(request.MaximumPrice);

                var result = await _unitOfWork.DapperProductRepository.GetAllBetweenPricesAsync(new Models.ProductModels.PriceBetweenModel(minimumPrice, maximumPrice));
                var resultData = _mapper.Map<ListGrpcProductModel>(result.Data);

                return resultData;
            });

        return result;
    }

    public override Task<ListGrpcProductModel> GetAllPagedAsync(GrpcPagingModel request, ServerCallContext context)
    {
        return base.GetAllPagedAsync(request, context);
    }
}

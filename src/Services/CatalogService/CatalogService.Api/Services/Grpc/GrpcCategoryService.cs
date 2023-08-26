using AutoMapper;
using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CacheModels;
using CatalogService.Api.Services.Cache.Abstract;
using CatalogService.Api.Services.Grpc.Abstract;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace CatalogService.Api.Services.Grpc;

public class GrpcCategoryService : BaseGrpcCategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRedisService _redisService;
    private readonly RedisOptions _redisOptions;

    public GrpcCategoryService(
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

    public override async Task<GrpcCategoryModel> GetAsync(GrpcIntModel request, ServerCallContext context)
    {
        var model = _mapper.Map<IntModel>(request);

        var result = await _unitOfWork.DapperCategoryRepository.GetAsync(model);
        var resultModel = _mapper.Map<GrpcCategoryModel>(result.Data);

        return resultModel;
    }

    public override async Task<GrpcCategoryModel> GetByNameAsync(GrpcStringModel request, ServerCallContext context)
    {
        var model = _mapper.Map<StringModel>(request);

        var result = await _unitOfWork.DapperCategoryRepository.GetByName(model);
        var resultModel = _mapper.Map<GrpcCategoryModel>(result.Data);

        return resultModel;
    }

    public override async Task<GrpcCategory> GetByNameWithProductsAsync(GrpcStringModel request, ServerCallContext context)
    {
        var model = _mapper.Map<StringModel>(request);

        var result = await _unitOfWork.DapperCategoryRepository.GetByNameWithProducts(model);
        var resultModel = _mapper.Map<GrpcCategory>(result.Data);

        return resultModel;
    }

    public override async Task<GrpcCategory> GetWithProductsAsync(GrpcIntModel request, ServerCallContext context)
    {
        var model = _mapper.Map<IntModel>(request);

        var result = await _unitOfWork.DapperCategoryRepository.GetWithProducts(model);
        var resultModel = _mapper.Map<GrpcCategory>(result.Data);

        return resultModel;
    }

    public override async Task<ListGrpcCategoryModel> GetAllAsync(GrpcEmptyModel request, ServerCallContext context)
    {
        var cacheKey = this.CurrentCacheKey(nameof(GetAllAsync));
        var result = await _redisService.GetAsync<ListGrpcCategoryModel>(
            cacheKey,
            _redisOptions.DatabaseId,
            _redisOptions.Duration,
            async () =>
            {
                var result = await _unitOfWork.DapperCategoryRepository.GetAllAsync();
                var resultModel = _mapper.Map<ListGrpcCategoryModel>(result.Data);

                return resultModel;
            });

        return result;
    }

    public override async Task<ListGrpcCategoryModel> GetAllPagedAsync(GrpcPagingModel request, ServerCallContext context)
    {
        var cacheKey = this.CurrentCacheKey(methodName: nameof(GetAllPagedAsync),
                                            parameters: new string[] { request.Page.ToString(), request.PageSize.ToString() });
        var result = await _redisService.GetAsync<ListGrpcCategoryModel>(
            cacheKey,
            _redisOptions.DatabaseId,
            _redisOptions.Duration,
            async () =>
            {
                var requestModel = _mapper.Map<PagingModel>(request);
                var result = await _unitOfWork.DapperCategoryRepository.GetAllPagedAsync(requestModel);
                var resultModel = _mapper.Map<ListGrpcCategoryModel>(result.Data);

                return resultModel;
            });

        return result;
    }

    public override async Task<ListGrpcCategoryModel> GetAllByParentIdAsync(GrpcIntModel request, ServerCallContext context)
    {
        var cacheKey = this.CurrentCacheKey(methodName: nameof(GetAllByParentIdAsync), parameters: request.Value.ToString());
        var result = await _redisService.GetAsync<ListGrpcCategoryModel>(
            cacheKey,
            _redisOptions.DatabaseId,
            _redisOptions.Duration,
            async () =>
            {
                var model = _mapper.Map<IntModel>(request);

                var result = await _unitOfWork.DapperCategoryRepository.GetAllByParentId(model);
                var resultModel = _mapper.Map<ListGrpcCategoryModel>(result.Data);

                return resultModel;
            });

        return result;
    }

    public override async Task<ListGrpcCategory> GetAllWithProductsByParentId(GrpcIntModel request, ServerCallContext context)
    {
        var cacheKey = this.CurrentCacheKey(methodName: nameof(GetAllWithProductsByParentId), parameters: request.Value.ToString());
        var result = await _redisService.GetAsync<ListGrpcCategory>(
            cacheKey,
            _redisOptions.DatabaseId,
            _redisOptions.Duration,
            async () =>
            {
                var model = _mapper.Map<IntModel>(request);

                var result = await _unitOfWork.DapperCategoryRepository.GetAllWithProductsByParentId(model);
                var resultModel = _mapper.Map<ListGrpcCategory>(result.Data);

                return resultModel;
            });

        return result;
    }
}

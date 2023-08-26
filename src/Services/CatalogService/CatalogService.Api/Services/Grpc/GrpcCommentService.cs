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

public class GrpcCommentService : BaseGrpcCommentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IRedisService _redisService;
    private RedisOptions _redisOptions;

    public GrpcCommentService(
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

    public override async Task<GrpcCommentModel> GetAsync(GrpcIntModel request, ServerCallContext context)
    {
        var model = _mapper.Map<IntModel>(request);

        var result = await _unitOfWork.DapperCommentRepository.GetAsync(model);
        var resultModel = _mapper.Map<GrpcCommentModel>(result.Data);

        return resultModel;
    }

    public override async Task<GrpcCommentModel> GetByCodeAsync(GrpcStringModel request, ServerCallContext context)
    {
        var model = _mapper.Map<StringModel>(request);

        var result = await _unitOfWork.DapperCommentRepository.GetByCodeAsync(model);
        var resultModel = _mapper.Map<GrpcCommentModel>(result.Data);

        return resultModel;
    }
    public override async Task<ListGrpcCommentModel> GetAllAsync(GrpcEmptyModel request, ServerCallContext context)
    {
        var cacheKey = this.CurrentCacheKey(nameof(GetAllAsync));
        var result = await _redisService.GetAsync<ListGrpcCommentModel>(
            cacheKey,
            _redisOptions.DatabaseId,
            _redisOptions.Duration,
            async () =>
            {
                var result = await _unitOfWork.DapperCommentRepository.GetAllAsync();
                var resultModel = _mapper.Map<ListGrpcCommentModel>(result.Data);

                return resultModel;
            });

        return result;
    }

    public override async Task<ListGrpcComment> GetAllByProductCode(GrpcIntModel request, ServerCallContext context)
    {
        var cacheKey = this.CurrentCacheKey(methodName: nameof(GetAllByProductCode), parameters: request.Value.ToString());
        var result = await _redisService.GetAsync<ListGrpcComment>(
            cacheKey,
            _redisOptions.DatabaseId,
            _redisOptions.Duration,
            async () =>
            {
                var model = _mapper.Map<IntModel>(request);

                var result = await _unitOfWork.DapperCommentRepository.GetAllByProductCode(model);
                var resultModel = _mapper.Map<ListGrpcComment>(result.Data);

                return resultModel;
            });

        return result;
    }

    public override async Task<ListGrpcCommentModel> GetAllByUserId(GrpcStringModel request, ServerCallContext context)
    {
        var cacheKey = this.CurrentCacheKey(methodName: nameof(GetAllByUserId), parameters: request.Value.ToString());
        var result = await _redisService.GetAsync<ListGrpcCommentModel>(
            cacheKey,
            _redisOptions.DatabaseId,
            _redisOptions.Duration,
            async () =>
            {
                var model = _mapper.Map<StringModel>(request);

                var result = await _unitOfWork.DapperCommentRepository.GetAllByUserId(model);
                var resultModel = _mapper.Map<ListGrpcCommentModel>(result.Data);

                return resultModel;
            });

        return result;
    }

    public override Task<ListGrpcCommentModel> GetAllPagedAsync(GrpcPagingModel request, ServerCallContext context)
    {
        return base.GetAllPagedAsync(request, context);
    }
}

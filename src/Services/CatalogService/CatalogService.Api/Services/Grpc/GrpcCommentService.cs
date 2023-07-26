using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.Dapper.Concrete;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Services.Grpc.Abstract;
using Grpc.Core;

namespace CatalogService.Api.Services.Grpc
{
    public class GrpcCommentService : BaseGrpcCommentService
    {
        private readonly IDapperCommentRepository _dapperCommentRepository;
        private readonly IMapper _mapper;

        public GrpcCommentService(
            IDapperCommentRepository dapperCommentRepository, 
            IMapper mapper)
        {
            _dapperCommentRepository = dapperCommentRepository;
            _mapper = mapper;
        }

        public override async Task<GrpcCommentModel> GetAsync(GrpcIntModel request, ServerCallContext context)
        {
            var model = _mapper.Map<IntModel>(request);

            var result = await _dapperCommentRepository.GetAsync(model);
            var resultModel = _mapper.Map<GrpcCommentModel>(result.Data);

            return resultModel;
        }

        public override async Task<GrpcCommentModel> GetByCodeAsync(GrpcStringModel request, ServerCallContext context)
        {
            var model = _mapper.Map<StringModel>(request);

            var result = await _dapperCommentRepository.GetByCodeAsync(model);
            var resultModel = _mapper.Map<GrpcCommentModel>(result.Data);

            return resultModel;
        }
        public override async Task<ListGrpcCommentModel> GetAllAsync(GrpcEmptyModel request, ServerCallContext context)
        {
            var result = await _dapperCommentRepository.GetAllAsync();
            var resultModel = _mapper.Map<ListGrpcCommentModel>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcComment> GetAllByProductId(GrpcIntModel request, ServerCallContext context)
        {
            var model = _mapper.Map<IntModel>(request);

            var result = await _dapperCommentRepository.GetAllByProductId(model);
            var resultModel = _mapper.Map<ListGrpcComment>(result.Data);

            return resultModel;
        }

        public override async Task<ListGrpcCommentModel> GetAllByUserId(GrpcStringModel request, ServerCallContext context)
        {
            var model = _mapper.Map<StringModel>(request);

            var result = await _dapperCommentRepository.GetAllByUserId(model);
            var resultModel = _mapper.Map<ListGrpcCommentModel>(result.Data);

            return resultModel;
        }
    }
}

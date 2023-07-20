using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Data.Repositories.EntityFramework.Abstract;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace CatalogService.Api.Services.Grpc.Concrete
{
    public class GrpcBrandService : BrandProtoService.BrandProtoServiceBase
    {
        private readonly IDapperBrandRepository _dapperBrandRepository;
        private readonly IEfBrandRepository _efBrandRepository;
        private readonly IMapper _mapper;

        public GrpcBrandService(
            IDapperBrandRepository dapperBrandRepository,
            IEfBrandRepository efBrandRepository,
            IMapper mapper)
        {
            _dapperBrandRepository = dapperBrandRepository;
            _efBrandRepository = efBrandRepository;
            _mapper = mapper;
        }

        public override async Task<GrpcResponseModel> AddAsync(GrpcBrandAddUpdateModel request, ServerCallContext context)
        {
            var addModel = _mapper.Map<Brand>(request);
            var result = await _efBrandRepository.AddAsync(addModel);

            var resultModel = _mapper.Map<GrpcResponseModel>(result);
            return await Task.FromResult(resultModel);
        }

        public override async Task<GrpcResponseModel> UpdateAsync(GrpcBrandAddUpdateModel request, ServerCallContext context)
        {
            var updateModel = _mapper.Map<Brand>(request);
            var result = await _efBrandRepository.UpdateAsync(updateModel);

            var resultModel = _mapper.Map<GrpcResponseModel>(result);
            return await Task.FromResult(resultModel);
        }

        public async override Task<GrpcResponseModel> DeleteAsync(GrpcIntModel request, ServerCallContext context)
        {
            var deleteModel = _mapper.Map<IntModel>(request);
            var result = await _efBrandRepository.DeleteAsync(deleteModel);

            var resultModel = _mapper.Map<GrpcResponseModel>(result);
            return await Task.FromResult(resultModel);
        }

        public override async Task<GrpcBrand> GetAsync(GrpcIntModel request, ServerCallContext context)
        {
            var requestModel = _mapper.Map<IntModel>(request);
            var result = await _dapperBrandRepository.GetAsync(requestModel);

            var resultData = _mapper.Map<GrpcBrand>(result.Data);
            return await Task.FromResult(resultData);
        }

        public override async Task<ListGrpcBrand> GetAllAsync(GrpcEmptyModel request, ServerCallContext context)
        {
            var result = await _dapperBrandRepository.GetAllAsync();
            var resultData = 

            return await Task.FromResult(resultData);
        }

        public override Task<ListGrpcBrand> GetAllWithProductsAsync(GrpcEmptyModel request, ServerCallContext context)
        {
            return base.GetAllWithProductsAsync(request, context);
        }
    }
}

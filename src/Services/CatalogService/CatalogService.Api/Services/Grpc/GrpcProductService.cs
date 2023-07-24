using AutoMapper;
using CatalogService.Api.Data.Repositories.Dapper.Abstract;
using CatalogService.Api.Models.Base.Concrete;
using Grpc.Core;

namespace CatalogService.Api.Services.Grpc
{
    public class GrpcProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly IDapperProductRepository _dapperProductRepository;
        private readonly IMapper _mapper;

        public GrpcProductService(
            IDapperProductRepository dapperProductRepository, 
            IMapper mapper)
        {
            _dapperProductRepository = dapperProductRepository;
            _mapper = mapper;
        }

        public override async Task<ListGrpcProductModel> GetAllAsync(GrpcEmptyModel request, ServerCallContext context)
        {
            var result = await _dapperProductRepository.GetAllAsync();
            var resultData = _mapper.Map<ListGrpcProductModel>(result.Data);

            return resultData;
        }

        public override async Task<ListGrpcProductModel> GetAllBetweenPricesAsync(GrpcPriceBetweenModel request, ServerCallContext context)
        {
            var minimumPrice = Convert.ToDecimal(request.MinimumPrice);
            var maximumPrice = Convert.ToDecimal(request.MaximumPrice);

            var result = await _dapperProductRepository.GetAllBetweenPricesAsync(new Models.ProductModels.PriceBetweenModel(minimumPrice, maximumPrice));
            var resultData = _mapper.Map<ListGrpcProductModel>(result.Data);

            return resultData;
        }

        public override async Task<GrpcProductModel> GetByProductCodeAsync(GrpcStringModel request, ServerCallContext context)
        {
            var requestData = _mapper.Map<StringModel>(request);

            var result = await _dapperProductRepository.GetByProductCodeAsync(requestData);
            var resultData = _mapper.Map<GrpcProductModel>(result.Data);

            return resultData;
        }
    }
}

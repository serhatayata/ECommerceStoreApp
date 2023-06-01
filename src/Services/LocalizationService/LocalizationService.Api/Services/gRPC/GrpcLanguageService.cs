using AutoMapper;
using Grpc.Core;
using LocalizationService.Api.Grpc;
using LocalizationService.Api.Models.Base.Concrete;
using LocalizationService.Api.Models.LanguageModels;
using LocalizationService.Api.Services.Abstract;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Services.gRPC
{
    public class GrpcLanguageService : LanguageGrpcService.LanguageGrpcServiceBase
    {
        private readonly ILanguageService _languageService;
        private readonly IMapper _mapper;

        public GrpcLanguageService(ILanguageService languageService,
                                   IMapper mapper)
        {
            _languageService = languageService;
            _mapper = mapper;
        }

        public async override Task<ResultGrpc> AddAsync(LanguageAddGrpcModel model, ServerCallContext context)
        {
            var mappedModel = _mapper.Map<LanguageAddGrpcModel, LanguageAddModel>(model);

            var addResult = await _languageService.AddAsync(mappedModel);
            var result = _mapper.Map<Result, ResultGrpc> (addResult);
            return result;
        }

        public async override Task<ResultGrpc> UpdateAsync(LanguageUpdateGrpcModel model, ServerCallContext context)
        {
            var mappedModel = _mapper.Map<LanguageUpdateGrpcModel, LanguageUpdateModel>(model);

            var addResult = await _languageService.UpdateAsync(mappedModel);
            var result = _mapper.Map<Result, ResultGrpc>(addResult);
            return result;
        }

        public async override Task<ResultGrpc> DeleteAsync(StringModelGrpc model, ServerCallContext context)
        {
            var mappedModel = _mapper.Map<StringModelGrpc, StringModel>(model);

            var deleteResult = await _languageService.DeleteAsync(mappedModel);
            var result = _mapper.Map<Result, ResultGrpc>(deleteResult);
            return result;
        }


    }
}

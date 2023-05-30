using AutoMapper;
using LocalizationService.Api.Grpc;
using LocalizationService.Api.Models.LanguageModels;
using LocalizationService.Api.Utilities.Results;

namespace LocalizationService.Api.Mapping
{
    public class GrpcMapProfile : Profile
    {
        public GrpcMapProfile()
        {
            #region Language
            CreateMap<LanguageAddModel, LanguageAddGrpcModel>().ReverseMap();
            #endregion
            #region Member

            #endregion
            #region Resource

            #endregion
            #region Results
            CreateMap<Result, ResultGrpc>().ReverseMap();
            #endregion
        }
    }
}

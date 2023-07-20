using AutoMapper;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        #region GENERAL
        CreateMap<IntModel, GrpcIntModel>().ReverseMap();
        CreateMap<StringModel, GrpcStringModel>().ReverseMap();
        CreateMap<BoolModel, GrpcBoolModel>().ReverseMap();
        CreateMap<PagingModel, GrpcPagingModel>().ReverseMap();


        CreateMap<Result, GrpcResponseModel>().ReverseMap();
        #endregion

        CreateMap<Brand, GrpcBrandAddUpdateModel>().ReverseMap();
        CreateMap<Brand, GrpcBrand>().ReverseMap();
    }
}

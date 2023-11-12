using AutoMapper;
using StockService.Api.Entities;
using StockService.Api.Models.StockModels;

namespace StockService.Api.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {

        CreateMap<Stock, StockAddModel>().ReverseMap();
        CreateMap<Stock, StockUpdateModel>().ReverseMap();
        CreateMap<Stock, StockModel>().ReverseMap();
        CreateMap<StockModel, StockUpdateModel>().ReverseMap();
    }
}

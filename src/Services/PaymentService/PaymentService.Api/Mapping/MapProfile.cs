using AutoMapper;

namespace PaymentService.Api.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        //CreateMap<AddressDto, AddressModel>().ReverseMap();

        //CreateMap<OrderAddModel, OrderCreateDto>().ReverseMap()
        //                                          .ForMember(o => o.FailMessage, opt => opt.Ignore())
        //                                          .ForMember(o => o.Status,
        //                                                     m => m.MapFrom(u => OrderStatus.Suspend));
    }
}

using AutoMapper;
using OrderService.Api.DTOs;
using OrderService.Api.Entities;
using OrderService.Api.Models.Enums;
using OrderService.Api.Models.OrderItemModels;
using OrderService.Api.Models.OrderModels;

namespace OrderService.Api.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        #region Order
        CreateMap<Order, OrderModel>().ReverseMap();
        CreateMap<Order, OrderAddModel>().ReverseMap();
        CreateMap<Order, OrderUpdateModel>().ReverseMap();
        CreateMap<Address, AddressModel>().ReverseMap();

        CreateMap<OrderAddModel, OrderCreateDto>().ReverseMap()
                                                  .ForMember(o => o.Status, 
                                                             m => m.MapFrom(u => OrderStatus.Suspend));
        #endregion
        #region OrderItem
        CreateMap<OrderItem, OrderItemModel>().ReverseMap();

        #endregion
    }
}

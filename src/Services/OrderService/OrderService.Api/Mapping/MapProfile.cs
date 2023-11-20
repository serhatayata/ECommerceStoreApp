using AutoMapper;
using OrderService.Api.DTOs;
using OrderService.Api.Entities;
using OrderService.Api.Models.Enums;
using OrderService.Api.Models.OrderItemModels;
using OrderService.Api.Models.OrderModels;
using Shared.Queue.Models;

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
        CreateMap<AddressDto, AddressModel>().ReverseMap();

        CreateMap<OrderAddModel, OrderCreateDto>().ReverseMap()
                                                  .ForMember(o => o.FailMessage, opt => opt.Ignore())
                                                  .ForMember(o => o.Status, 
                                                             m => m.MapFrom(u => OrderStatus.Suspend));
        #endregion
        #region OrderItem
        CreateMap<OrderItem, OrderItemModel>().ReverseMap();
        CreateMap<PaymentMessage, PaymentDto>().ReverseMap();
        CreateMap<OrderItemMessage, OrderItemDto>().ReverseMap();
        CreateMap<OrderItemMessage, OrderItemDto>().ReverseMap();

        #endregion
    }
}

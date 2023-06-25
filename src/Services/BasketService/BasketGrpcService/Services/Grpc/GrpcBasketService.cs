using BasketGrpcService.Models;
using BasketGrpcService.Protos;
using BasketGrpcService.Repositories.Abstract;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace BasketGrpcService.Services.Grpc
{
    public class GrpcBasketService : BasketProtoService.BasketProtoServiceBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<GrpcBasketService> _logger;

        public GrpcBasketService(IBasketRepository basketRepository, 
                                 ILogger<GrpcBasketService> logger)
        {
            _basketRepository = basketRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        public override async Task<CustomerBasketResponse> GetBasketById(BasketRequest request, ServerCallContext context)
        {
            throw new Exception("test1");

            var data = await _basketRepository.GetBasketAsync(request.Id);

            if (data != null)
            {
                context.Status = new Status(StatusCode.OK, $"Basket with id {request.Id} do exist");

                return MapToCustomerBasketResponse(data);
            }
            else
            {
                context.Status = new Status(StatusCode.NotFound, $"Basket with id {request.Id} do not exist");
            }

            return new CustomerBasketResponse();
        }

        public override async Task<CustomerBasketResponse> UpdateBasket(CustomerBasketRequest request, ServerCallContext context)
        {
            var customerBasket = MapToCustomerBasket(request);

            var response = await _basketRepository.UpdateBasketAsync(customerBasket);

            if (response != null)
            {
                return MapToCustomerBasketResponse(response);
            }

            context.Status = new Status(StatusCode.NotFound, $"Basket with buyer id {request.Buyerid} do not exist");

            return null;
        }

        private CustomerBasketResponse MapToCustomerBasketResponse(CustomerBasket customerBasket)
        {
            var response = new CustomerBasketResponse
            {
                Buyerid = customerBasket.BuyerId
            };

            customerBasket.Items.ForEach(item => response.Items.Add(new BasketItemResponse
            {
                Id = item.Id,
                Oldunitprice = (double)item.OldUnitPrice,
                Pictureurl = item.PictureUrl,
                Productid = item.ProductId,
                Productname = item.ProductName,
                Quantity = item.Quantity,
                Unitprice = (double)item.UnitPrice
            }));

            return response;
        }

        private CustomerBasket MapToCustomerBasket(CustomerBasketRequest customerBasketRequest)
        {
            var response = new CustomerBasket
            {
                BuyerId = customerBasketRequest.Buyerid
            };

            customerBasketRequest.Items.ToList().ForEach(item => response.Items.Add(new BasketItem
            {
                Id = item.Id,
                OldUnitPrice = (decimal)item.Oldunitprice,
                PictureUrl = item.Pictureurl,
                ProductId = item.Productid,
                ProductName = item.Productname,
                Quantity = item.Quantity,
                UnitPrice = (decimal)item.Unitprice
            }));

            return response;
        }
    }
}

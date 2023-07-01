using BasketService.Api.Models;

namespace BasketService.Api.Repositories.Abstract
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string customerId);
        List<string> GetUsers();
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}

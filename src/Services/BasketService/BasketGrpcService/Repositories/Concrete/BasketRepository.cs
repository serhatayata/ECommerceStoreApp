using BasketGrpcService.Models;
using BasketGrpcService.Repositories.Abstract;
using BasketGrpcService.Services.Redis.Abstract;
using Microsoft.Extensions.Options;

namespace BasketGrpcService.Repositories.Concrete
{
    public class BasketRepository : IBasketRepository
    {
        private readonly ILogger<BasketRepository> _logger;
        private readonly IRedisService _redisService;
        private readonly RedisOptions _redisOptions;

        public BasketRepository(ILogger<BasketRepository> logger, 
                                IRedisService redisService,
                                IOptions<RedisOptions> redisOptions)
        {
            _logger = logger;
            _redisService = redisService;

            _redisOptions = redisOptions.Value;
        }

        public async Task<bool> DeleteBasketAsync(string id)
        {
            var key = $"{_redisOptions.Prefix}{id}";

            return await _redisService.RemoveAsync(key, _redisOptions.DatabaseId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string customerId)
        {
            var data = await _redisService.GetAsync<CustomerBasket>(this.PrefixKey(customerId), _redisOptions.DatabaseId);
            return data;
        }

        public List<string> GetUsers()
        {
            var keys = _redisService.GetKeys(_redisOptions.Prefix, _redisOptions.DatabaseId);
            var users = Array.ConvertAll(keys, x => x.ToString());

            return users.ToList();
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            var isCreated = await _redisService.SetAsync(key: this.PrefixKey(basket.BuyerId), 
                                                         value: basket, 
                                                         duration: 24,
                                                         databaseId: _redisOptions.DatabaseId);

            if (!isCreated)
            {
                _logger.LogError("Problem occured while creating basket item, BuyerId : {BuyerId}", basket.BuyerId);
                return null;
            }

            return await GetBasketAsync(basket.BuyerId);
        }

        private string PrefixKey(string key)
        {
            return $"{_redisOptions.Prefix}{key}";
        }
    }
}

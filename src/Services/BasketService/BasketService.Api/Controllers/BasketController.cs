using BasketService.Api.IntegrationEvents.Events;
using BasketService.Api.Models;
using BasketService.Api.Repositories.Abstract;
using BasketService.Api.Services.Identity.Abstract;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace BasketService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IIdentityService _identityService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepository basketRepository, 
                                IIdentityService identityService, 
                                IEventBus eventBus, 
                                ILogger<BasketController> logger)
        {
            _basketRepository = basketRepository;
            _identityService = identityService;
            _eventBus = eventBus;
            _logger = logger;
        }

        [Route("get-basket-byid")]
        [HttpGet]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketByIdAsync([FromQuery] StringModel model)
        {
            var basket = await _basketRepository.GetBasketAsync(model.Value);

            return Ok(basket ?? new CustomerBasket(model.Value));
        }

        [Route("update-basket")]
        [HttpPost]
        [ProducesResponseType(typeof(CustomerBasket), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasketAsync([FromBody] CustomerBasket value)
        {
            return Ok(await _basketRepository.UpdateBasketAsync(value));
        }

        [Route("checkout")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutAsync([FromBody] BasketCheckout basketCheckout, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var userId = _identityService.GetUserIdentity();
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest();

            basketCheckout.RequestId = (Guid.TryParse(requestId, out Guid guid) && guid != Guid.Empty) ?
                guid : basketCheckout.RequestId;

            var basket = await _basketRepository.GetBasketAsync(userId);

            if (basket == null)
                return BadRequest();

            var userName = this.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Name)?.Value;

            var eventMessage = new UserCheckoutAcceptedIntegrationEvent(userId, userName, basketCheckout.City, basketCheckout.Street,
                basketCheckout.State, basketCheckout.Country, basketCheckout.ZipCode, basketCheckout.CardNumber, basketCheckout.CardHolderName,
                basketCheckout.CardExpiration, basketCheckout.CardSecurityNumber, basketCheckout.CardTypeId, basketCheckout.Buyer, basketCheckout.RequestId, basket);

            // Once basket is checkout, sends an integration event to
            // ordering.api to convert basket to order and proceeds with
            // order creation process
            try
            {
                _eventBus.Publish(eventMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}", eventMessage.Id, Program.appName);

                throw;
            }

            return Accepted();
        }

        // DELETE api/values/5
        [Route("delete-basket")]
        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task DeleteBasketByIdAsync([FromBody] StringModel model)
        {
            await _basketRepository.DeleteBasketAsync(model.Value);
        }
    }
}

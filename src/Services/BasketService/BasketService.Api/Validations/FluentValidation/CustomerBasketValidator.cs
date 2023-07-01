using BasketService.Api.Extensions;
using BasketService.Api.Models;
using BasketService.Api.Services.Localization.Abstract;
using BasketService.Api.Validations.FluentValidation.gRPC;
using FluentValidation;

namespace BasketService.Api.Validations.FluentValidation
{
    public class CustomerBasketValidator : AbstractValidator<CustomerBasket>
    {
        public CustomerBasketValidator(ILocalizationService localizer,
                                       IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(request => request.BuyerId).NotEmpty().WithMessage(localizer[culture, "customerbasket.buyerid.notempty"]);

            RuleFor(request => request.BuyerId).NotNull().WithMessage(localizer[culture, "customerbasket.buyerid.notnull"]);

            RuleForEach(request => request.Items).SetValidator(new BasketItemValidator(localizer, httpContextAccessor));
        }
    }
}

using BasketGrpcService.Extensions;
using BasketGrpcService.Models;
using BasketGrpcService.Services.Localization.Abstract;
using BasketGrpcService.Validations.FluentValidation.gRPC;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation
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

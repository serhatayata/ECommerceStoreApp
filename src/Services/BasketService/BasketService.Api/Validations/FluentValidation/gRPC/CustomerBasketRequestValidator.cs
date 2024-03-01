using BasketService.Api.Extensions;
using BasketService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace BasketService.Api.Validations.FluentValidation.gRPC
{
    public class CustomerBasketRequestValidator : AbstractValidator<CustomerBasketRequest>
    {
        public CustomerBasketRequestValidator(ILocalizationService localizer,
                                              IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(request => request.Buyerid).NotEmpty().WithMessage(l => localizer[culture, "customerbasketrequest.buyerid.notempty"]);
            RuleFor(request => request.Buyerid).NotNull().WithMessage(l => localizer[culture, "customerbasketrequest.buyerid.notnull"]);

            RuleForEach(request => request.Items).SetValidator(new BasketItemResponseValidator(localizer, httpContextAccessor));
        }
    }
}

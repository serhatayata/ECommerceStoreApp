using BasketGrpcService.Extensions;
using BasketGrpcService.Services.Localization.Abstract;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation.gRPC
{
    public class CustomerBasketRequestValidator : AbstractValidator<CustomerBasketRequest>
    {
        public CustomerBasketRequestValidator(ILocalizationService localizer,
                                              IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(request => request.Buyerid).NotEmpty().WithMessage(localizer[culture, "customerbasketrequest.buyerid.notempty"]);
            RuleFor(request => request.Buyerid).NotNull().WithMessage(localizer[culture, "customerbasketrequest.buyerid.notnull"]);

            RuleForEach(request => request.Items).SetValidator(new BasketItemResponseValidator(localizer, httpContextAccessor));
        }
    }
}

using BasketService.Api.Extensions;
using BasketService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace BasketService.Api.Validations.FluentValidation.gRPC
{
    public class BasketRequestValidator : AbstractValidator<BasketRequest>
    {
        public BasketRequestValidator(ILocalizationService localizer,
                                      IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(request => request.Id).NotEmpty().WithMessage(l => localizer[culture, "basketrequest.id.notempty"]);
            RuleFor(request => request.Id).NotNull().WithMessage(l => localizer[culture, "basketrequest.id.notnull"]);
        }
    }
}

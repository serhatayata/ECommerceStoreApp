using BasketGrpcService.Extensions;
using BasketGrpcService.Services.Localization.Abstract;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation.gRPC
{
    public class BasketRequestValidator : AbstractValidator<BasketRequest>
    {
        public BasketRequestValidator(ILocalizationService localizer,
                                      IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(request => request.Id).NotEmpty().WithMessage(localizer[culture, "basketrequest.id.notempty"]);
            RuleFor(request => request.Id).NotNull().WithMessage(localizer[culture, "basketrequest.id.notnull"]);
        }
    }
}

using BasketGrpcService.Extensions;
using BasketGrpcService.Models;
using BasketGrpcService.Services.Localization.Abstract;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation.Base
{
    public class StringModelValidator : AbstractValidator<StringModel>
    {
        public StringModelValidator(ILocalizationService localizer,
                                    IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(request => request.Value).NotEmpty().WithMessage(localizer[culture, "stringmodel.value.notempty"]);
            RuleFor(request => request.Value).NotNull().WithMessage(localizer[culture, "stringmodel.value.notnull"]);
        }
    }
}

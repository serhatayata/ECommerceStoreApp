using BasketService.Api.Extensions;
using BasketService.Api.Models;
using BasketService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace BasketService.Api.Validations.FluentValidation.Base
{
    public class StringModelValidator : AbstractValidator<StringModel>
    {
        public StringModelValidator(ILocalizationService localizer,
                                    IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(request => request.Value).NotEmpty().WithMessage(l => localizer[culture, "stringmodel.value.notempty"]);
            RuleFor(request => request.Value).NotNull().WithMessage(l => localizer[culture, "stringmodel.value.notnull"]);
        }
    }
}

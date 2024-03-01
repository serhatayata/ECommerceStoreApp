using CatalogService.Api.Extensions;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.Base;

public class IntModelValidator : AbstractValidator<IntModel>
{
    public IntModelValidator(ILocalizationService localizer,
                             IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(b => b.Value).NotEmpty().NotNull().WithMessage(l => localizer[culture, "intmodel.name.notempty"]);
    }
}

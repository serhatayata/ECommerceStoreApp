using CatalogService.Api.Extensions;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.ProductValidators;

public class PriceBetweenModelValidator : AbstractValidator<PriceBetweenModel>
{
    public PriceBetweenModelValidator(ILocalizationService localizer,
                                      IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(b => b.MinimumPrice).NotEmpty().NotNull().WithMessage(localizer[culture, "pricebetweenmodel.minimumprice.notempty"]);
        RuleFor(b => b.MinimumPrice).GreaterThan(0).WithMessage(localizer[culture, "pricebetweenmodel.minimumprice.greater", 0]);

        RuleFor(b => b.MaximumPrice).NotEmpty().NotNull().WithMessage(localizer[culture, "pricebetweenmodel.maximumprice.notempty"]);
        RuleFor(b => b.MaximumPrice).GreaterThan(0).WithMessage(localizer[culture, "pricebetweenmodel.maximumprice.greater"]);
    }
}

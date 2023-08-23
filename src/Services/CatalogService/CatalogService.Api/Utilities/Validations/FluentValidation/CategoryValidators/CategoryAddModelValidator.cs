using CatalogService.Api.Extensions;
using CatalogService.Api.Models.CategoryModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CategoryValidators;

public class CategoryAddModelValidator : AbstractValidator<CategoryAddModel>
{
    public CategoryAddModelValidator(ILocalizationService localizer,
                                     IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage(localizer[culture, "categoryaddmodel.name.notempty"]);
        RuleFor(b => b.Name).Length(2, 500).WithMessage(localizer[culture, "categoryaddmodel.name.length", 2, 500]);

        RuleFor(b => b.Line).NotEmpty().NotNull().WithMessage(localizer[culture, "categoryaddmodel.line.notempty"]);
        RuleFor(b => b.Line).GreaterThan(0).WithMessage(localizer[culture, "categoryaddmodel.line.greater", 0]);
    }
}

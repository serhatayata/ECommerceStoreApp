using CatalogService.Api.Extensions;
using CatalogService.Api.Models.BrandModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.BrandValidators
{
    public class BrandAddModelValidator : AbstractValidator<BrandAddModel>
    {
        public BrandAddModelValidator(ILocalizationService localizer,
                                      IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage(localizer[culture, "brandaddmodel.name.notempty"]);
            RuleFor(b => b.Name).Length(2, 500).WithMessage("Name length must be between 2-500");

            RuleFor(b => b.Description).NotEmpty().NotNull().WithMessage("Description cannot be empty");
            RuleFor(b => b.Description).Length(5, 500).WithMessage("Description length must be between 5-500");
        }
    }
}

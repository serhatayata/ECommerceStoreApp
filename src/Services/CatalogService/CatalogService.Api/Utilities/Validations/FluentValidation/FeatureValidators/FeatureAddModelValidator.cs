using CatalogService.Api.Extensions;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.FeatureValidators
{
    public class FeatureAddModelValidator : AbstractValidator<FeatureAddModel>
    {
        public FeatureAddModelValidator(ILocalizationService localizer,
                                        IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage(l => localizer[culture, "featureaddmodel.name.notempty"]);
            RuleFor(b => b.Name).Length(2, 100).WithMessage(l => localizer[culture, "featureaddmodel.name.length", 2, 100]);
        }
    }
}

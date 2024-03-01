using CatalogService.Api.Extensions;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.FeatureValidators
{
    public class FeatureUpdateModelValidator : AbstractValidator<FeatureUpdateModel>
    {
        public FeatureUpdateModelValidator(ILocalizationService localizer,
                                           IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.Id).NotEmpty().NotNull().WithMessage(l => localizer[culture, "featureupdatemodel.id.notempty"]);
            RuleFor(b => b.Id).GreaterThan(0).WithMessage(l => localizer[culture, "featureupdatemodel.id.greater", 0]);

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage(l => localizer[culture, "featureupdatemodel.name.notempty"]);
            RuleFor(b => b.Name).Length(2, 100).WithMessage(l => localizer[culture, "featureupdatemodel.name.length", 2, 100]);
        }
    }
}

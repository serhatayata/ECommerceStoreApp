using CatalogService.Api.Extensions;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.FeatureValidators
{
    public class ProductFeatureModelValidator : AbstractValidator<ProductFeatureModel>
    {
        public ProductFeatureModelValidator(ILocalizationService localizer,
                                            IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.FeatureId).NotEmpty().NotNull().WithMessage(l => localizer[culture, "productfeaturemodel.featureid.notempty"]);
            RuleFor(b => b.FeatureId).GreaterThan(0).WithMessage(l => localizer[culture, "productfeaturemodel.featureid.greater", 0]);

            RuleFor(b => b.ProductId).NotEmpty().NotNull().WithMessage(l => localizer[culture, "productfeaturemodel.productid.notempty"]);
            RuleFor(b => b.ProductId).GreaterThan(0).WithMessage(l => localizer[culture, "productfeaturemodel.productid.notempty", 0]);
        }
    }
}

using CatalogService.Api.Extensions;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.FeatureValidators
{
    public class ProductFeaturePropertyAddModelValidator : AbstractValidator<ProductFeaturePropertyAddModel>
    {
        public ProductFeaturePropertyAddModelValidator(ILocalizationService localizer,
                                                       IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.ProductFeatureId).NotEmpty().NotNull().WithMessage(l => localizer[culture, "productfeaturepropertyaddmodel.productfeatureid.notempty"]);
            RuleFor(b => b.ProductFeatureId).GreaterThan(0).WithMessage(l => localizer[culture, "productfeaturepropertyaddmodel.productfeatureid.greater",0]);

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage(l => localizer[culture, "productfeaturepropertyaddmodel.name.notempty"]);
            RuleFor(b => b.Name).Length(2, 100).WithMessage(l => localizer[culture, "productfeaturepropertyaddmodel.name.greater", 2, 100]);

            RuleFor(b => b.Description).NotEmpty().NotNull().WithMessage(l => localizer[culture, "productfeaturepropertyaddmodel.description.notempty"]);
            RuleFor(b => b.Description).Length(2, 1000).WithMessage(l => localizer[culture, "productfeaturepropertyaddmodel.description.length", 0]);
        }
    }
}

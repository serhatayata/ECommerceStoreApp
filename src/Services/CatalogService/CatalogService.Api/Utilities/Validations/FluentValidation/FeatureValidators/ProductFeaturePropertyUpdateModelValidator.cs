using CatalogService.Api.Extensions;
using CatalogService.Api.Models.FeatureModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.FeatureValidators
{
    public class ProductFeaturePropertyUpdateModelValidator : AbstractValidator<ProductFeaturePropertyUpdateModel>
    {
        public ProductFeaturePropertyUpdateModelValidator(ILocalizationService localizer,
                                                          IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.Id).NotEmpty().NotNull().WithMessage(localizer[culture, "productfeaturepropertyupdatemodel.id.notempty"]);
            RuleFor(b => b.Id).GreaterThan(0).WithMessage(localizer[culture, "productfeaturepropertyupdatemodel.id.greater", 0]);

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage(localizer[culture, "productfeaturepropertyupdatemodel.name.notempty"]);
            RuleFor(b => b.Name).Length(2, 100).WithMessage(localizer[culture, "productfeaturepropertyupdatemodel.name.notempty", 2, 100]);

            RuleFor(b => b.Description).NotEmpty().NotNull().WithMessage(localizer[culture, "productfeaturepropertyupdatemodel.description.notempty"]);
            RuleFor(b => b.Description).Length(2, 1000).WithMessage(localizer[culture, "productfeaturepropertyupdatemodel.description.length", 2, 1000]);
        }
    }
}

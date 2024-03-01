using CatalogService.Api.Extensions;
using CatalogService.Api.Models.BrandModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.BrandValidators
{
    public class BrandUpdateModelValidator : AbstractValidator<BrandUpdateModel>
    {
        public BrandUpdateModelValidator(ILocalizationService localizer,
                                         IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.Id).NotEmpty().NotNull().WithMessage(l => localizer[culture, "brandupdatemodel.id.notempty"]);
            RuleFor(b => b.Id).GreaterThan(0).WithMessage(l => localizer[culture, "brandupdatemodel.id.greater", 0]);

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage(l => localizer[culture, "brandupdatemodel.name.notempty"]);
            RuleFor(b => b.Name).Length(2, 500).WithMessage(l => localizer[culture, "brandupdatemodel.name.length", 2, 500]);

            RuleFor(b => b.Description).NotEmpty().NotNull().WithMessage(l => localizer[culture, "brandupdatemodel.description.notempty"]);
            RuleFor(b => b.Description).Length(5, 500).WithMessage(l => localizer[culture, "brandupdatemodel.description.length", 5, 500]);
        }
    }
}

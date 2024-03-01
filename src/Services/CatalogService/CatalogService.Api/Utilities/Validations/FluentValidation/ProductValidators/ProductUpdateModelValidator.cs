using CatalogService.Api.Extensions;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.ProductValidators
{
    public class ProductUpdateModelValidator : AbstractValidator<ProductUpdateModel>
    {
        public ProductUpdateModelValidator(ILocalizationService localizer,
                                           IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.Id).NotEmpty().NotNull().WithMessage(l => localizer[culture, "productupdatemodel.id.notempty"]);
            RuleFor(b => b.Id).GreaterThan(0).WithMessage(l => localizer[culture, "productupdatemodel.name.greaterthan", 0]);

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage(l => localizer[culture, "productupdatemodel.name.notempty"]);
            RuleFor(b => b.Name).Length(2, 100).WithMessage(l => localizer[culture, "productupdatemodel.name.length", 2, 100]);

            RuleFor(b => b.Description).NotEmpty().NotNull().WithMessage(l => localizer[culture, "productupdatemodel.description.notempty"]);
            RuleFor(b => b.Description).Length(2, 1000).WithMessage(l => localizer[culture, "productupdatemodel.description.length", 2, 1000]);

            RuleFor(b => b.Price).NotEmpty().NotNull().WithMessage(l => localizer[culture, "productupdatemodel.price.notempty"]);
            RuleFor(b => b.Price).PrecisionScale(8, 2, true).WithMessage(l => localizer[culture, "productupdatemodel.price.precision", 8, 2]);

            RuleFor(b => b.AvailableStock).NotEmpty().NotNull().WithMessage(l => localizer[culture, "productupdatemodel.availablestock.notempty"]);
        }
    }
}

using CatalogService.Api.Extensions;
using CatalogService.Api.Models.ProductModels;
using CatalogService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.ProductValidators
{
    public class ProductAddModelValidator : AbstractValidator<ProductAddModel>
    {
        public ProductAddModelValidator(ILocalizationService localizer,
                                        IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage(localizer[culture, "productaddmodel.name.notempty"]);
            RuleFor(b => b.Name).Length(2, 100).WithMessage(localizer[culture, "productaddmodel.name.greaterthan", 0]);

            RuleFor(b => b.Description).NotEmpty().NotNull().WithMessage(localizer[culture, "productaddmodel.description.notempty"]);
            RuleFor(b => b.Description).Length(2, 1000).WithMessage(localizer[culture, "productaddmodel.description.length", 2, 1000]);

            RuleFor(b => b.Price).NotEmpty().NotNull().WithMessage(localizer[culture, "productaddmodel.name.notempty"]);
            RuleFor(b => b.Price).PrecisionScale(8,2, true).WithMessage(localizer[culture, "productaddmodel.price.precision", 8, 2]);

            RuleFor(b => b.AvailableStock).NotEmpty().NotNull().WithMessage(localizer[culture, "productaddmodel.name.availablestock"]);
        }
    }
}

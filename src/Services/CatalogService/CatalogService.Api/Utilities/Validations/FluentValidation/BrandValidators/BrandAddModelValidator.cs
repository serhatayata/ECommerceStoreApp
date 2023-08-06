using CatalogService.Api.Models.BrandModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.BrandValidators
{
    public class BrandAddModelValidator : AbstractValidator<BrandAddModel>
    {
        public BrandAddModelValidator()
        {
            //RuleFor(b => b.Value).NotEmpty().WithMessage("Value cannot be empty");
        }
    }
}

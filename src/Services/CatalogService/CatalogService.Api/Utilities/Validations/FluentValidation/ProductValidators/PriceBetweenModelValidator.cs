using CatalogService.Api.Models.ProductModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.ProductValidators
{
    public class PriceBetweenModelValidator : AbstractValidator<PriceBetweenModel>
    {
        public PriceBetweenModelValidator()
        {
            RuleFor(b => b.MinimumPrice).NotEmpty().NotNull().WithMessage("Minimum price cannot be empty");
            RuleFor(b => b.MinimumPrice).GreaterThan(0).WithMessage("Minimum price must be greater than 0");

            RuleFor(b => b.MaximumPrice).NotEmpty().NotNull().WithMessage("Maximum price cannot be empty");
            RuleFor(b => b.MaximumPrice).GreaterThan(0).WithMessage("Maximum price must be greater than 0");
        }
    }
}

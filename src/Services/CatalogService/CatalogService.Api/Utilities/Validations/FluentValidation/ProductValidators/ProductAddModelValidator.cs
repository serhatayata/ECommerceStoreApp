using CatalogService.Api.Models.ProductModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.ProductValidators
{
    public class ProductAddModelValidator : AbstractValidator<ProductAddModel>
    {
        public ProductAddModelValidator()
        {
            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage("Name cannot be empty");
            RuleFor(b => b.Name).Length(2, 100).WithMessage("Name must be greater than 0");

            RuleFor(b => b.Description).NotEmpty().NotNull().WithMessage("Description cannot be empty");
            RuleFor(b => b.Description).Length(2, 1000).WithMessage("Description must be greater than 0");

            RuleFor(b => b.Price).NotEmpty().NotNull().WithMessage("Price cannot be empty");
            RuleFor(b => b.Price).PrecisionScale(8,2, true).WithMessage("Price cannot have more than 8 numbers");

            RuleFor(b => b.AvailableStock).NotEmpty().NotNull().WithMessage("Available stock cannot be empty");
        }
    }
}

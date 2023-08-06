using CatalogService.Api.Models.FeatureModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.FeatureValidators
{
    public class ProductFeatureModelValidator : AbstractValidator<ProductFeatureModel>
    {
        public ProductFeatureModelValidator()
        {
            RuleFor(b => b.FeatureId).NotEmpty().NotNull().WithMessage("Feature Id cannot be empty");
            RuleFor(b => b.FeatureId).GreaterThan(0).WithMessage("Feature Id must be greater than 0");

            RuleFor(b => b.ProductId).NotEmpty().NotNull().WithMessage("Product Id cannot be empty");
            RuleFor(b => b.ProductId).GreaterThan(0).WithMessage("Product Id must be greater than 0");
        }
    }
}

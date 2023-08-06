using CatalogService.Api.Models.FeatureModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.FeatureValidators
{
    public class FeatureAddModelValidator : AbstractValidator<FeatureAddModel>
    {
        public FeatureAddModelValidator()
        {
            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage("Name cannot be empty");
            RuleFor(b => b.Name).Length(2, 100).WithMessage("Name must be greater than 0");
        }
    }
}

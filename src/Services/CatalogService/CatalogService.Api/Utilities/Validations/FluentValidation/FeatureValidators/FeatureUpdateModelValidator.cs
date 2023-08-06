using CatalogService.Api.Models.FeatureModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.FeatureValidators
{
    public class FeatureUpdateModelValidator : AbstractValidator<FeatureUpdateModel>
    {
        public FeatureUpdateModelValidator()
        {
            RuleFor(b => b.Id).NotEmpty().NotNull().WithMessage("Id cannot be empty");
            RuleFor(b => b.Id).GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage("Name cannot be empty");
            RuleFor(b => b.Name).Length(2, 100).WithMessage("Name must be greater than 0");
        }
    }
}

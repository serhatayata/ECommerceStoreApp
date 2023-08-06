using CatalogService.Api.Models.BrandModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.BrandValidators
{
    public class BrandUpdateModelValidator : AbstractValidator<BrandUpdateModel>
    {
        public BrandUpdateModelValidator()
        {
            RuleFor(b => b.Id).NotEmpty().NotNull().WithMessage("Id cannot be empty");
            RuleFor(b => b.Id).GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage("Name cannot be empty");
            RuleFor(b => b.Name).Length(2, 500).WithMessage("Name length must be between 2-500");

            RuleFor(b => b.Description).NotEmpty().NotNull().WithMessage("Description cannot be empty");
            RuleFor(b => b.Description).Length(5, 500).WithMessage("Description length must be between 5-500");
        }
    }
}

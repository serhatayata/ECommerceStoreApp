using CatalogService.Api.Models.CategoryModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CategoryValidators
{
    public class CategoryAddModelValidator : AbstractValidator<CategoryAddModel>
    {
        public CategoryAddModelValidator()
        {
            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage("Name cannot be empty");
            RuleFor(b => b.Name).Length(2, 500).WithMessage("Name length must be between 2-500");

            RuleFor(b => b.Line).NotEmpty().NotNull().WithMessage("Line cannot be empty");
            RuleFor(b => b.Line).GreaterThan(0).WithMessage("Line cannot be empty");
        }
    }
}

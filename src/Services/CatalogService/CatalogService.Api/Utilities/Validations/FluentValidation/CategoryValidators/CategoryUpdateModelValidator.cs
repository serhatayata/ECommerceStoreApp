using CatalogService.Api.Models.CategoryModels;
using FluentValidation;

namespace CatalogService.Api.Utilities.Validations.FluentValidation.CategoryValidators
{
    public class CategoryUpdateModelValidator : AbstractValidator<CategoryUpdateModel>
    {
        public CategoryUpdateModelValidator()
        {
            RuleFor(b => b.Id).NotEmpty().NotNull().WithMessage("Id cannot be empty");
            RuleFor(b => b.Id).GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(b => b.Name).NotEmpty().NotNull().WithMessage("Name cannot be empty");
            RuleFor(b => b.Name).Length(2, 500).WithMessage("Name length must be between 2-500");

            RuleFor(b => b.Line).NotEmpty().NotNull().WithMessage("Line cannot be empty");
            RuleFor(b => b.Line).GreaterThan(0).WithMessage("Line cannot be empty");
        }
    }
}

using FluentValidation;
using LocalizationService.Api.Models.ResourceModels;

namespace LocalizationService.Api.Validations.FluentValidation.ResourceValidators
{
    public class ResourceUpdateModelValidator : AbstractValidator<ResourceUpdateModel>
    {
        public ResourceUpdateModelValidator()
        {
            RuleFor(m => m.Tag).NotEmpty().WithMessage("Tag cannot be empty");
            RuleFor(m => m.Tag).NotNull().WithMessage("Tag cannot be null");
            RuleFor(m => m.Tag).Length(5, 300).WithMessage("Tag length must be between 5-300");

            RuleFor(m => m.Value).NotEmpty().WithMessage("Value cannot be empty");
            RuleFor(m => m.Value).NotNull().WithMessage("Value cannot be null");

            RuleFor(m => m.ResourceCode).NotEmpty().WithMessage("Resource code cannot be empty");
            RuleFor(m => m.ResourceCode).NotNull().WithMessage("Resource code cannot be null");
            RuleFor(m => m.ResourceCode).Length(0, 40).WithMessage("Resource code length must be between 0-40");
        }
    }
}

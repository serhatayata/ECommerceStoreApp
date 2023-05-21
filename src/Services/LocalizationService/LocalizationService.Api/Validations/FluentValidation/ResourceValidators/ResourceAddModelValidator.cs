using FluentValidation;
using LocalizationService.Api.Models.ResourceModels;

namespace LocalizationService.Api.Validations.FluentValidation.ResourceValidators
{
    public class ResourceAddModelValidator : AbstractValidator<ResourceAddModel>
    {
        public ResourceAddModelValidator()
        {
            RuleFor(m => m.Tag).NotEmpty().WithMessage("Tag cannot be empty");
            RuleFor(m => m.Tag).NotNull().WithMessage("Tag cannot be null");
            RuleFor(m => m.Tag).Length(5, 300).WithMessage("Tag length must be between 5-300");

            RuleFor(m => m.Value).NotEmpty().WithMessage("Value cannot be empty");
            RuleFor(m => m.Value).NotNull().WithMessage("Value cannot be null");

            RuleFor(m => m.MemberKey).NotEmpty().WithMessage("Member key cannot be empty");
            RuleFor(m => m.MemberKey).NotNull().WithMessage("Member key cannot be null");

            RuleFor(m => m.LanguageCode).NotEmpty().WithMessage("Language code cannot be empty");
            RuleFor(m => m.LanguageCode).NotNull().WithMessage("Language code cannot be null");
        }
    }
}

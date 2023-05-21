using FluentValidation;
using LocalizationService.Api.Models.LanguageModels;

namespace LocalizationService.Api.Validations.FluentValidation.LanguageValidators
{
    public class LanguageUpdateModelValidator : AbstractValidator<LanguageUpdateModel>
    {
        public LanguageUpdateModelValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code cannot be empty");
            RuleFor(x => x.Code).NotNull().WithMessage("Code cannot be null");

            RuleFor(x => x.DisplayName).NotEmpty().WithMessage("Display name cannot be empty");
            RuleFor(x => x.DisplayName).NotNull().WithMessage("Display name cannot be null");
            RuleFor(x => x.DisplayName).Length(1, 100).WithMessage("Display name length must be between 1-100");
        }
    }
}

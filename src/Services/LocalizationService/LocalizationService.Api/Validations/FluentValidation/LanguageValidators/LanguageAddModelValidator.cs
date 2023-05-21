using FluentValidation;
using LocalizationService.Api.Models.LanguageModels;

namespace LocalizationService.Api.Validations.FluentValidation.LanguageValidators
{
    public class LanguageAddModelValidator : AbstractValidator<LanguageAddModel>
    {
        public LanguageAddModelValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code cannot be empty");
            RuleFor(x => x.Code).NotNull().WithMessage("Code cannot be null");
            RuleFor(x => x.Code).Length(5,10).WithMessage("Code length must be between 5-10");

            RuleFor(x => x.DisplayName).NotEmpty().WithMessage("Display name cannot be empty");
            RuleFor(x => x.DisplayName).NotNull().WithMessage("Display name cannot be null");
            RuleFor(x => x.DisplayName).Length(1, 100).WithMessage("Display name length must be between 1-100");
        }
    }
}

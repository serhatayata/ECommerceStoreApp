using FluentValidation;
using LocalizationService.Api.Models.MemberModels;

namespace LocalizationService.Api.Validations.FluentValidation.MemberValidators
{
    public class MemberAddModelValidator : AbstractValidator<MemberAddModel>
    {
        public MemberAddModelValidator()
        {
            RuleFor(m => m.Name).NotEmpty().WithMessage("Name cannot be empty");
            RuleFor(m => m.Name).NotNull().WithMessage("Name cannot be null");
            RuleFor(m => m.Name).Length(5, 100).WithMessage("Name length must be between 5-100");
        }
    }
}

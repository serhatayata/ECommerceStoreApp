using FluentValidation;
using LocalizationService.Api.Models.Base.Concrete;

namespace LocalizationService.Api.Validations.FluentValidation.Base
{
    public class BoolModelValidator : AbstractValidator<BoolModel>
    {
        public BoolModelValidator()
        {
            RuleFor(b => b.Value).NotEmpty().WithMessage("Value cannot be empty");
            RuleFor(b => b.Value).NotNull().WithMessage("Value cannot be null");
        }
    }
}

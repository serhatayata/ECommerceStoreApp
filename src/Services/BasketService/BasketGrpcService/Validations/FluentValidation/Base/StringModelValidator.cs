using BasketGrpcService.Models;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation.Base
{
    public class StringModelValidator : AbstractValidator<StringModel>
    {
        public StringModelValidator()
        {
            RuleFor(request => request.Value).NotEmpty().WithMessage("Value is mandatory.");
            RuleFor(request => request.Value).NotNull().WithMessage("Value cannot be null.");
        }
    }
}

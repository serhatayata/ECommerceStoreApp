using BasketGrpcService.Models;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation.Base
{
    public class StringModelValidator : AbstractValidator<StringModel>
    {
        public StringModelValidator()
        {
            RuleFor(request => request.Value).NotEmpty().NotNull().WithMessage("Value is mandatory.");
        }
    }
}

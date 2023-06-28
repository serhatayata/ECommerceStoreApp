using BasketGrpcService.Models;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation
{
    public class BasketCheckoutValidator : AbstractValidator<BasketCheckout>
    {
        public BasketCheckoutValidator()
        {
            RuleFor(request => request.City).NotNull().WithMessage("City is mandatory.");
            RuleFor(request => request.City).NotEmpty().WithMessage("City is mandatory.");
            RuleFor(request => request.City).MinimumLength(0).WithMessage("City length must be greater than 0");

            RuleFor(request => request.Street).NotNull().WithMessage("Street is mandatory.");
            RuleFor(request => request.Street).NotEmpty().WithMessage("Street is mandatory.");
            RuleFor(request => request.Street).MinimumLength(0).WithMessage("Street length must be greater than 0");

            RuleFor(request => request.State).NotEmpty().WithMessage("State is mandatory.");
            RuleFor(request => request.State).NotNull().WithMessage("State is mandatory.");
            RuleFor(request => request.State).MinimumLength(0).WithMessage("State length must be greater than 0");

            RuleFor(request => request.ZipCode).NotNull().WithMessage("State is mandatory.");
            RuleFor(request => request.ZipCode).NotEmpty().WithMessage("State is mandatory.");
            RuleFor(request => request.ZipCode).MinimumLength(0).WithMessage("State length must be greater than 0");

            RuleFor(request => request.CardNumber).NotEmpty().WithMessage("Card number is mandatory.");
            RuleFor(request => request.CardNumber).NotNull().WithMessage("Card number is mandatory.");
            RuleFor(request => request.CardNumber).MinimumLength(0).WithMessage("Card number length must be greater than 0");

            RuleFor(request => request.CardHolderName).NotEmpty().WithMessage("Card holder name is mandatory.");
            RuleFor(request => request.CardHolderName).NotNull().WithMessage("Card holder name is mandatory.");
            RuleFor(request => request.CardHolderName).MinimumLength(0).WithMessage("Card holder name length must be greater than 0");

            RuleFor(request => request.CardExpiration).NotNull().WithMessage("Card holder name is mandatory.");
            RuleFor(request => request.CardExpiration).NotEmpty().WithMessage("Card holder name is mandatory.");

            RuleFor(request => request.CardSecurityNumber).NotEmpty().WithMessage("Card security number is mandatory.");
            RuleFor(request => request.CardSecurityNumber).NotNull().WithMessage("Card security number is mandatory.");
            RuleFor(request => request.CardSecurityNumber).MinimumLength(0).WithMessage("Card security number length must be greater than 0");

            RuleFor(request => request.CardTypeId).NotNull().WithMessage("Card type id is mandatory.");
            RuleFor(request => request.CardTypeId).NotEmpty().WithMessage("Card type id is mandatory.");

            RuleFor(request => request.Buyer).NotNull().WithMessage("Buyer is mandatory.");
            RuleFor(request => request.Buyer).NotEmpty().WithMessage("Buyer is mandatory.");
            RuleFor(request => request.Buyer).MinimumLength(0).WithMessage("Buyer length must be greater than 0");

            RuleFor(request => request.RequestId).NotEmpty().WithMessage("RequestId is mandatory.");
            RuleFor(request => request.RequestId).NotNull().WithMessage("RequestId is mandatory.");
        }
    }
}

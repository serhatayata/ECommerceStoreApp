using BasketGrpcService.Models;
using BasketGrpcService.Validations.FluentValidation.gRPC;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation
{
    public class CustomerBasketValidator : AbstractValidator<CustomerBasket>
    {
        public CustomerBasketValidator()
        {
            RuleFor(request => request.BuyerId).NotEmpty().WithMessage("Buyer id is mandatory.");

            RuleFor(request => request.BuyerId).NotNull().WithMessage("Buyer id is mandatory.");

            RuleForEach(request => request.Items).SetValidator(new BasketItemValidator());
        }
    }
}

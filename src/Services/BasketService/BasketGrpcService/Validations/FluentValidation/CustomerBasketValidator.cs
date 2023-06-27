using BasketGrpcService.Models;
using BasketGrpcService.Validations.FluentValidation.gRPC;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation
{
    public class CustomerBasketValidator : AbstractValidator<CustomerBasket>
    {
        public CustomerBasketValidator()
        {
            RuleFor(request => request.BuyerId).NotEmpty().NotNull().WithMessage("Buyer id is mandatory.");

            RuleForEach(request => request.Items).SetValidator(new BasketItemValidator());
        }
    }
}

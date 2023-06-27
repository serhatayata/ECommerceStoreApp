using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation.gRPC
{
    public class CustomerBasketRequestValidator : AbstractValidator<CustomerBasketRequest>
    {
        public CustomerBasketRequestValidator()
        {
            RuleFor(request => request.Buyerid).NotEmpty().NotNull().WithMessage("Buyer id is mandatory.");

            RuleForEach(request => request.Items).SetValidator(new BasketItemResponseValidator());
        }
    }
}

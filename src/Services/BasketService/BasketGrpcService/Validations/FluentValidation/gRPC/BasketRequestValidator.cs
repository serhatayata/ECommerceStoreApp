using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation.gRPC
{
    public class BasketRequestValidator : AbstractValidator<BasketRequest>
    {
        public BasketRequestValidator()
        {
            RuleFor(request => request.Id).NotEmpty().NotNull().WithMessage("Id is mandatory.");
        }
    }
}

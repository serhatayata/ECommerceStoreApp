using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation.gRPC
{
    public class BasketRequestValidator : AbstractValidator<BasketRequest>
    {
        public BasketRequestValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("Id is mandatory.");
            RuleFor(request => request.Id).NotNull().WithMessage("Id is mandatory.");
        }
    }
}

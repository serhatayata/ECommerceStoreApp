using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation.gRPC;

public class BasketItemResponseValidator : AbstractValidator<BasketItemResponse>
{
    public BasketItemResponseValidator()
    {
        RuleFor(request => request.Id).NotEmpty().NotNull().WithMessage("Id is mandatory.");

        RuleFor(request => request.Productid).NotEmpty().NotNull().WithMessage("Product id is mandatory.");

        RuleFor(request => request.Productname).NotEmpty().NotNull().WithMessage("Product name is mandatory.");
        RuleFor(request => request.Productname).MinimumLength(0).WithMessage("Product name length must be greater than 1");

        RuleFor(request => request.Unitprice).NotNull().NotEmpty().WithMessage("Unit price cannot be null");
        RuleFor(request => request.Unitprice).GreaterThan(0).WithMessage("Unit price must be greater than 0");

        RuleFor(request => request.Oldunitprice).NotNull().NotEmpty().WithMessage("Old unit price cannot be null");
        RuleFor(request => request.Oldunitprice).GreaterThan(0).WithMessage("Old unit price must be greater than 0");

        RuleFor(request => request.Quantity).NotNull().NotEmpty().WithMessage("Quantity cannot be null");
        RuleFor(request => request.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

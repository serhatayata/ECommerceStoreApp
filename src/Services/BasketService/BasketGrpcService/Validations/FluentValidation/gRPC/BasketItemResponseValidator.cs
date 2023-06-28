using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation.gRPC;

public class BasketItemResponseValidator : AbstractValidator<BasketItemResponse>
{
    public BasketItemResponseValidator()
    {
        RuleFor(request => request.Id).NotEmpty().WithMessage("Id is mandatory.");
        RuleFor(request => request.Id).NotNull().WithMessage("Id cannot be null.");

        RuleFor(request => request.Productid).NotEmpty().WithMessage("Product id is mandatory.");
        RuleFor(request => request.Productid).NotNull().WithMessage("Product id cannot be null.");

        RuleFor(request => request.Productname).NotEmpty().WithMessage("Product name is mandatory.");
        RuleFor(request => request.Productname).NotNull().WithMessage("Product name cannot be null.");
        RuleFor(request => request.Productname).MinimumLength(0).WithMessage("Product name length must be greater than 1");

        RuleFor(request => request.Unitprice).NotNull().WithMessage("Unit price cannot be null");
        RuleFor(request => request.Unitprice).NotEmpty().WithMessage("Unit price cannot be null");
        RuleFor(request => request.Unitprice).GreaterThan(0).WithMessage("Unit price must be greater than 0");

        RuleFor(request => request.Oldunitprice).NotEmpty().WithMessage("Old unit price cannot be null");
        RuleFor(request => request.Oldunitprice).NotNull().WithMessage("Old unit price cannot be null");
        RuleFor(request => request.Oldunitprice).GreaterThan(0).WithMessage("Old unit price must be greater than 0");

        RuleFor(request => request.Quantity).NotEmpty().WithMessage("Quantity cannot be null");
        RuleFor(request => request.Quantity).NotNull().WithMessage("Quantity cannot be null");
        RuleFor(request => request.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}

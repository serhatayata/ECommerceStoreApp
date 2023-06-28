using BasketGrpcService.Models;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation
{
    public class BasketItemValidator : AbstractValidator<BasketItem>
    {
        public BasketItemValidator()
        {
            RuleFor(request => request.Id).NotEmpty().WithMessage("Id is mandatory.");
            RuleFor(request => request.Id).NotNull().WithMessage("Id is mandatory.");

            RuleFor(request => request.ProductId).NotEmpty().WithMessage("Product id is mandatory.");
            RuleFor(request => request.ProductId).NotNull().WithMessage("Product id is mandatory.");

            RuleFor(request => request.ProductName).NotEmpty().WithMessage("Product name is mandatory.");
            RuleFor(request => request.ProductName).NotNull().WithMessage("Product name is mandatory.");
            RuleFor(request => request.ProductName).MinimumLength(0).WithMessage("Product name length must be greater than 1");

            RuleFor(request => request.UnitPrice).NotNull().WithMessage("Unit price cannot be null");
            RuleFor(request => request.UnitPrice).NotEmpty().WithMessage("Unit price cannot be null");
            RuleFor(request => request.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than 0");

            RuleFor(request => request.OldUnitPrice).NotEmpty().WithMessage("Old unit price cannot be null");
            RuleFor(request => request.OldUnitPrice).NotNull().WithMessage("Old unit price cannot be null");
            RuleFor(request => request.OldUnitPrice).GreaterThan(0).WithMessage("Old unit price must be greater than 0");

            RuleFor(request => request.Quantity).NotNull().WithMessage("Quantity cannot be null");
            RuleFor(request => request.Quantity).NotEmpty().WithMessage("Quantity cannot be null");
            RuleFor(request => request.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}

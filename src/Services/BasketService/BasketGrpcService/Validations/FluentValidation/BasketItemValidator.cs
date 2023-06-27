using BasketGrpcService.Models;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation
{
    public class BasketItemValidator : AbstractValidator<BasketItem>
    {
        public BasketItemValidator()
        {
            RuleFor(request => request.Id).NotEmpty().NotNull().WithMessage("Id is mandatory.");

            RuleFor(request => request.ProductId).NotEmpty().NotNull().WithMessage("Product id is mandatory.");

            RuleFor(request => request.ProductName).NotEmpty().NotNull().WithMessage("Product name is mandatory.");
            RuleFor(request => request.ProductName).MinimumLength(0).WithMessage("Product name length must be greater than 1");

            RuleFor(request => request.UnitPrice).NotNull().NotEmpty().WithMessage("Unit price cannot be null");
            RuleFor(request => request.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than 0");

            RuleFor(request => request.OldUnitPrice).NotNull().NotEmpty().WithMessage("Old unit price cannot be null");
            RuleFor(request => request.OldUnitPrice).GreaterThan(0).WithMessage("Old unit price must be greater than 0");

            RuleFor(request => request.Quantity).NotNull().NotEmpty().WithMessage("Quantity cannot be null");
            RuleFor(request => request.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0");
        }
    }
}

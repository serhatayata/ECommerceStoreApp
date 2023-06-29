using BasketGrpcService.Extensions;
using BasketGrpcService.Models;
using BasketGrpcService.Services.Localization.Abstract;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation
{
    public class BasketItemValidator : AbstractValidator<BasketItem>
    {
        public BasketItemValidator(ILocalizationService localizer,
                                   IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(request => request.Id).NotEmpty().WithMessage(localizer[culture, "basketitem.id.notempty"]);
            RuleFor(request => request.Id).NotNull().WithMessage(localizer[culture, "basketitem.id.notnull"]);

            RuleFor(request => request.ProductId).NotEmpty().WithMessage(localizer[culture, "basketitem.productid.notempty"]);
            RuleFor(request => request.ProductId).NotNull().WithMessage(localizer[culture, "basketitem.productid.notnull"]);

            RuleFor(request => request.ProductName).NotEmpty().WithMessage(localizer[culture, "basketitem.productname.notempty"]);
            RuleFor(request => request.ProductName).NotNull().WithMessage(localizer[culture, "basketitem.productname.notnull"]);
            RuleFor(request => request.ProductName).MinimumLength(0).WithMessage(localizer[culture, "basketitem.productname.minimumlength",0]);

            RuleFor(request => request.UnitPrice).NotNull().WithMessage(localizer[culture, "basketitem.unitprice.notnull"]);
            RuleFor(request => request.UnitPrice).NotEmpty().WithMessage(localizer[culture, "basketitem.unitprice.notempty"]);
            RuleFor(request => request.UnitPrice).GreaterThan(0).WithMessage(localizer[culture, "basketitem.unitprice.greaterthan",0]);

            RuleFor(request => request.OldUnitPrice).NotEmpty().WithMessage(localizer[culture, "basketitem.oldunitprice.notempty"]);
            RuleFor(request => request.OldUnitPrice).NotNull().WithMessage(localizer[culture, "basketitem.oldunitprice.notnull"]);
            RuleFor(request => request.OldUnitPrice).GreaterThan(0).WithMessage(localizer[culture, "basketitem.oldunitprice.greaterthan",0]);

            RuleFor(request => request.Quantity).NotNull().WithMessage(localizer[culture, "basketitem.quantity.notnull"]);
            RuleFor(request => request.Quantity).NotEmpty().WithMessage(localizer[culture, "basketitem.quantity.notempty"]);
            RuleFor(request => request.Quantity).GreaterThan(0).WithMessage(localizer[culture, "basketitem.quantity.greaterthan",0]);
        }
    }
}

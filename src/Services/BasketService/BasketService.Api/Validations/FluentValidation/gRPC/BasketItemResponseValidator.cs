using BasketService.Api.Extensions;
using BasketService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace BasketService.Api.Validations.FluentValidation.gRPC;

public class BasketItemResponseValidator : AbstractValidator<BasketItemResponse>
{
    public BasketItemResponseValidator(ILocalizationService localizer,
                                       IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(request => request.Id).NotEmpty().WithMessage(l => localizer[culture, "basketitemresponse.id.notempty"]);
        RuleFor(request => request.Id).NotNull().WithMessage(l => localizer[culture, "basketitemresponse.id.notnull"]);

        RuleFor(request => request.Productid).NotEmpty().WithMessage(l => localizer[culture, "basketitemresponse.productid.notempty"]);
        RuleFor(request => request.Productid).NotNull().WithMessage(l => localizer[culture, "basketitemresponse.productid.notnull"]);

        RuleFor(request => request.Productname).NotEmpty().WithMessage(l => localizer[culture, "basketitemresponse.productname.notempty"]);
        RuleFor(request => request.Productname).NotNull().WithMessage(l => localizer[culture, "basketitemresponse.productname.notnull"]);
        RuleFor(request => request.Productname).MinimumLength(0).WithMessage(l => localizer[culture, "basketitemresponse.productname.minimumlength",0]);

        RuleFor(request => request.Unitprice).NotNull().WithMessage(l => localizer[culture, "basketitemresponse.unitprice.notnull"]);
        RuleFor(request => request.Unitprice).NotEmpty().WithMessage(l => localizer[culture, "basketitemresponse.unitprice.notempty"]);
        RuleFor(request => request.Unitprice).GreaterThan(0).WithMessage(l => localizer[culture, "basketitemresponse.unitprice.greaterthan", 0]);

        RuleFor(request => request.Oldunitprice).NotEmpty().WithMessage(l => localizer[culture, "basketitemresponse.oldunitprice.notempty"]);
        RuleFor(request => request.Oldunitprice).NotNull().WithMessage(l => localizer[culture, "basketitemresponse.oldunitprice.notnull"]);
        RuleFor(request => request.Oldunitprice).GreaterThan(0).WithMessage(l => localizer[culture, "basketitemresponse.oldunitprice.greaterthan", 0]);

        RuleFor(request => request.Quantity).NotEmpty().WithMessage(l => localizer[culture, "basketitemresponse.quantity.notempty"]);
        RuleFor(request => request.Quantity).NotNull().WithMessage(l => localizer[culture, "basketitemresponse.quantity.notnull"]);
        RuleFor(request => request.Quantity).GreaterThan(0).WithMessage(l => localizer[culture, "basketitemresponse.quantity.greaterthan", 0]);
    }
}

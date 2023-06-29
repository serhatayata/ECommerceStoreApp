using BasketGrpcService.Extensions;
using BasketGrpcService.Models;
using BasketGrpcService.Services.Localization.Abstract;
using FluentValidation;

namespace BasketGrpcService.Validations.FluentValidation
{
    public class BasketCheckoutValidator : AbstractValidator<BasketCheckout>
    {
        public BasketCheckoutValidator(ILocalizationService localizer,
                                       IHttpContextAccessor httpContextAccessor)
        {
            string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

            RuleFor(request => request.City).NotNull().WithMessage(localizer[culture, "basketcheckout.city.notnull"]);
            RuleFor(request => request.City).NotEmpty().WithMessage(localizer[culture, "basketcheckout.city.notempty"]);
            RuleFor(request => request.City).MinimumLength(0).WithMessage(localizer[culture, "basketcheckout.city.minimumlength", 0]);

            RuleFor(request => request.Street).NotNull().WithMessage(localizer[culture, "basketcheckout.street.notnull"]);
            RuleFor(request => request.Street).NotEmpty().WithMessage(localizer[culture, "basketcheckout.street.notempty"]);
            RuleFor(request => request.Street).MinimumLength(0).WithMessage(localizer[culture, "basketcheckout.street.minimumlength",0]);

            RuleFor(request => request.State).NotEmpty().WithMessage(localizer[culture, "basketcheckout.state.notempty"]);
            RuleFor(request => request.State).NotNull().WithMessage(localizer[culture, "basketcheckout.state.notnull"]);
            RuleFor(request => request.State).MinimumLength(0).WithMessage(localizer[culture, "basketcheckout.state.minimumlength",0]);

            RuleFor(request => request.ZipCode).NotNull().WithMessage(localizer[culture, "basketcheckout.zipcode.notnull"]);
            RuleFor(request => request.ZipCode).NotEmpty().WithMessage(localizer[culture, "basketcheckout.zipcode.notempty"]);
            RuleFor(request => request.ZipCode).MinimumLength(0).WithMessage(localizer[culture, "basketcheckout.zipcode.minimumlength",0]);

            RuleFor(request => request.CardNumber).NotEmpty().WithMessage(localizer[culture, "basketcheckout.cardnumber.notempty"]);
            RuleFor(request => request.CardNumber).NotNull().WithMessage(localizer[culture, "basketcheckout.cardnumber.notnull"]);
            RuleFor(request => request.CardNumber).MinimumLength(0).WithMessage(localizer[culture, "basketcheckout.cardnumber.minimumlength",0]);

            RuleFor(request => request.CardHolderName).NotEmpty().WithMessage(localizer[culture, "basketcheckout.cardholdername.notempty"]);
            RuleFor(request => request.CardHolderName).NotNull().WithMessage(localizer[culture, "basketcheckout.cardholdername.notnull"]);
            RuleFor(request => request.CardHolderName).MinimumLength(0).WithMessage(localizer[culture, "basketcheckout.cardholdername.minimumlength",0]);

            RuleFor(request => request.CardExpiration).NotNull().WithMessage(localizer[culture, "basketcheckout.cardexpiration.notnull"]);
            RuleFor(request => request.CardExpiration).NotEmpty().WithMessage(localizer[culture, "basketcheckout.cardexpiration.notempty"]);

            RuleFor(request => request.CardSecurityNumber).NotEmpty().WithMessage(localizer[culture, "basketcheckout.cardsecuritynumber.notempty"]);
            RuleFor(request => request.CardSecurityNumber).NotNull().WithMessage(localizer[culture, "basketcheckout.cardsecuritynumber.notnull"]);
            RuleFor(request => request.CardSecurityNumber).MinimumLength(0).WithMessage(localizer[culture, "basketcheckout.cardsecuritynumber.minimumlength",0]);

            RuleFor(request => request.CardTypeId).NotNull().WithMessage(localizer[culture, "basketcheckout.cardtype.notnull"]);
            RuleFor(request => request.CardTypeId).NotEmpty().WithMessage(localizer[culture, "basketcheckout.cardtype.notempty"]);

            RuleFor(request => request.Buyer).NotNull().WithMessage(localizer[culture, "basketcheckout.buyer.notnull"]);
            RuleFor(request => request.Buyer).NotEmpty().WithMessage(localizer[culture, "basketcheckout.buyer.notempty"]);
            RuleFor(request => request.Buyer).MinimumLength(0).WithMessage(localizer[culture, "basketcheckout.buyer.minimumlength",0]);

            RuleFor(request => request.RequestId).NotEmpty().WithMessage(localizer[culture, "basketcheckout.requestid.notempty"]);
            RuleFor(request => request.RequestId).NotNull().WithMessage(localizer[culture, "basketcheckout.requestid.notnull"]);
        }
    }
}

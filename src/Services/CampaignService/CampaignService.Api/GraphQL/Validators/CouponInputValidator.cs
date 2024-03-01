using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CampaignService.Api.GraphQL.Validators;

public class CouponInputValidator : AbstractValidator<CouponInput>
{
    public CouponInputValidator(
        ILocalizationService localizer,
        IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(c => c.Name)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "couponinput.name.notempty"])
            .Length(1, 255).WithMessage(l => localizer[culture, "couponinput.name.length", 1, 255]);

        RuleFor(c => c.Description)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "couponinput.description.notempty"])
            .Length(1, 1000).WithMessage(l => localizer[culture, "couponinput.description.length", 1, 1000]);

        RuleFor(c => c.Type)
            .IsInEnum().WithMessage(l => localizer[culture, "couponinput.type.notvalid"]);

        RuleFor(c => c.UsageType)
            .IsInEnum().WithMessage(l => localizer[culture, "couponinput.usagetype.notvalid"]);

        RuleFor(c => c.CalculationType)
            .IsInEnum().WithMessage(l => localizer[culture, "couponinput.calculationtype.notvalid"]);

        RuleFor(c => c.CalculationAmount)
            .PrecisionScale(8, 2, true).WithMessage(l => localizer[culture, "couponinput.calculationamount.notvalid"]);

        RuleFor(c => c.Amount)
            .PrecisionScale(8, 2, true).WithMessage(l => localizer[culture, "couponinput.amount.notvalid"]);

        RuleFor(c => c.MaxUsage)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "couponinput.maxusage.notempty"]);

        RuleFor(c => c.ExpirationDate)
            .NotNull().NotEmpty().WithMessage((l => localizer[culture, "couponinput.expirationdate.notempty"]);
    }
}

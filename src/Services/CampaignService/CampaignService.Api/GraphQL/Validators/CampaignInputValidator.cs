using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CampaignService.Api.GraphQL.Validators;

public class CampaignInputValidator : AbstractValidator<CampaignInput>
{
    public CampaignInputValidator(
        ILocalizationService localizer,
        IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(c => c.Status)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigninput.status.notempty"]);

        RuleFor(c => c.Name)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigninput.name.notempty"])
            .Length(1,255).WithMessage(l => localizer[culture, "campaigninput.name.length", 1, 255]);

        RuleFor(c => c.Description)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigninput.description.notempty"])
            .Length(1,1000).WithMessage(l => localizer[culture, "campaigninput.description.length", 1, 1000]);

        RuleFor(c => c.ExpirationDate)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigninput.expirationdate.notempty"]);

        RuleFor(c => c.StartDate)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigninput.startdate.notempty"]);

        RuleFor(c => c.Sponsor)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigninput.sponsor.notempty"])
            .Length(1,255).WithMessage(l => localizer[culture, "campaigninput.sponsor.length", 1, 255]);

        RuleFor(c => c.DiscountType)
            .IsInEnum().WithMessage(l => localizer[culture, "campaigninput.discounttype.notempty"]);

        RuleFor(c => c.PlatformType)
            .IsInEnum().WithMessage(l => localizer[culture, "campaigninput.platformtype.notvalid"]);

        RuleFor(c => c.CalculationType)
            .IsInEnum().WithMessage(l => localizer[culture, "campaigninput.calculationtype.notvalid"]);

        RuleFor(c => c.CalculationAmount)
            .PrecisionScale(8, 2, true).WithMessage(l => localizer[culture, "campaigninput.calculationamount.notvalid"]);

        RuleFor(c => c.Amount)
            .PrecisionScale(8, 2, true).WithMessage(l => localizer[culture, "campaigninput.amount.notvalid"]);

        RuleFor(c => c.MaxUsage)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigninput.maxusage.notvalid"]);

        RuleFor(c => c.MaxUsagePerUser)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigninput.maxusageperuser.notvalid"]);
    }
}

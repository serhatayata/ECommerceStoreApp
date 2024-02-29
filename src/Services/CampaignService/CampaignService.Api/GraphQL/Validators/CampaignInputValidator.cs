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
            .NotNull().NotEmpty().WithMessage(localizer[culture, "campaigninput.status.notempty"]);

        RuleFor(c => c.Name)
            .NotNull().NotEmpty().WithMessage(localizer[culture, "campaigninput.name.notempty"])
            .Length(1,255).WithMessage(localizer[culture, "campaigninput.name.length", 1, 255]);

        RuleFor(c => c.Description)
            .NotNull().NotEmpty().WithMessage(localizer[culture, "campaigninput.description.notempty"])
            .Length(1,1000).WithMessage(localizer[culture, "campaigninput.description.length", 1, 1000]);

        RuleFor(c => c.ExpirationDate)
            .NotNull().NotEmpty().WithMessage(localizer[culture, "campaigninput.expirationdate.notempty"]);

        RuleFor(c => c.StartDate)
            .NotNull().NotEmpty().WithMessage(localizer[culture, "campaigninput.startdate.notempty"]);

        RuleFor(c => c.Sponsor)
            .NotNull().NotEmpty().WithMessage(localizer[culture, "campaigninput.sponsor.notempty"])
            .Length(1,255).WithMessage(localizer[culture, "campaigninput.sponsor.length", 1, 255]);

        RuleFor(c => c.DiscountType)
            .IsInEnum().WithMessage(localizer[culture, "campaigninput.discounttype.notempty"]);

        RuleFor(c => c.PlatformType)
            .IsInEnum().WithMessage(localizer[culture, "campaigninput.platformtype.notvalid"]);

        RuleFor(c => c.CalculationType)
            .IsInEnum().WithMessage(localizer[culture, "campaigninput.calculationtype.notvalid"]);

        RuleFor(c => c.CalculationAmount)
            .PrecisionScale(8, 2, true).WithMessage(localizer[culture, "campaigninput.calculationamount.notvalid"]);

        RuleFor(c => c.Amount)
            .PrecisionScale(8, 2, true).WithMessage(localizer[culture, "campaigninput.amount.notvalid"]);

        RuleFor(c => c.MaxUsage)
            .NotNull().NotEmpty().WithMessage(localizer[culture, "campaigninput.maxusage.notvalid"]);

        RuleFor(c => c.MaxUsagePerUser)
            .NotNull().NotEmpty().WithMessage(localizer[culture, "campaigninput.maxusageperuser.notvalid"]);
    }
}

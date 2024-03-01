using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.Services.Localization.Abstract;
using FluentValidation;


namespace CampaignService.Api.GraphQL.Validators;

public class CouponItemInputValidator : AbstractValidator<CouponItemInput>
{
    public CouponItemInputValidator(
        ILocalizationService localizer,
        IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(c => c.CouponId)
            .NotEmpty().NotNull().WithMessage(l => localizer[culture, "couponiteminput.couponid.notvalid"]);

        RuleFor(c => c.Status)
            .IsInEnum().WithMessage(l => localizer[culture, "couponiteminput.status.notvalid"]);
    }
}

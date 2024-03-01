using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CampaignService.Api.GraphQL.Validators;

public class CouponUsageInputValidator : AbstractValidator<CouponUsageInput>
{
    public CouponUsageInputValidator(
        ILocalizationService localizer,
        IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(c => c.Code)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "couponusageinput.code.notempty"])
            .Length(1, 40).WithMessage(l => localizer[culture, "couponusageinput.code.length", 1, 40]);

        RuleFor(c => c.UserId)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "couponusageinput.userid.notempty"])
            .Length(1, 40).WithMessage(l => localizer[culture, "couponusageinput.userid.length", 1, 40]);
    }
}

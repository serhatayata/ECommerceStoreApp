using CampaignService.Api.GraphQL.Types.Inputs;
using FluentValidation;
using CampaignService.Api.Services.Localization.Abstract;
using CampaignService.Api.Extensions;

namespace CampaignService.Api.GraphQL.Validators;

public class CampaignSourceInputValidator : AbstractValidator<CampaignSourceInput>
{
    public CampaignSourceInputValidator(
        ILocalizationService localizer,
        IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(c => c.EntityId)
            .NotEmpty().NotNull().WithMessage(l => localizer[culture, "campaignsourceinput.entityid.notempty"]);

        RuleFor(c => c.CampaignId)
            .NotEmpty().NotNull().WithMessage(l => localizer[culture, "campaignsourceinput.campaignid.notempty"]);
    }
}

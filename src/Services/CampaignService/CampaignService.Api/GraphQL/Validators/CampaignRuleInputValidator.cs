using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.Services.Localization.Abstract;
using FluentValidation;


namespace CampaignService.Api.GraphQL.Validators;

public class CampaignRuleInputValidator : AbstractValidator<CampaignRuleInput>
{
    public CampaignRuleInputValidator(
        ILocalizationService localizer,
        IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(c => c.CampaignId)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaignruleinput.campaignid.notempty"]);

        RuleFor(c => c.Type)
            .IsInEnum().WithMessage(l => localizer[culture, "campaignruleinput.type.notvalid"]);

        RuleFor(c => c.Data)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaignruleinput.data.notemtpy"])
            .Length(1, 10000).WithMessage(l => localizer[culture, "campaignruleinput.data.length", 1, 10000]);

        RuleFor(c => c.Value)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaignruleinput.value.notemtpy"])
            .Length(1, 10000).WithMessage(l => localizer[culture, "campaignruleinput.value.length", 1, 10000]);
    }
}

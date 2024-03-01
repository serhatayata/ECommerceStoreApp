using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.Services.Localization.Abstract;
using FluentValidation;

namespace CampaignService.Api.GraphQL.Validators;

public class CampaignItemInputValidator : AbstractValidator<CampaignItemInput>
{
    public CampaignItemInputValidator(
        ILocalizationService localizer,
        IHttpContextAccessor httpContextAccessor)
    {
        string culture = HttpExtensions.GetAcceptLanguage(httpContextAccessor);

        RuleFor(c => c.CampaignId)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigniteminput.campaignid.notempty"]);

        RuleFor(c => c.UserId)
            .Length(1, 40).WithMessage(l => localizer[culture, "campaigniteminput.campaignid.notempty"]);

        RuleFor(c => c.Description)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigniteminput.description.notempty"])
            .Length(1, 40).WithMessage(l => localizer[culture, "campaigniteminput.description.length", 1, 40]);

        RuleFor(c => c.Status)
            .IsInEnum().WithMessage(l => localizer[culture, "campaigniteminput.status.notvalid", 1, 40]);

        RuleFor(c => c.ExpirationDate)
            .NotNull().NotEmpty().WithMessage(l => localizer[culture, "campaigniteminput.expirationdate.notempty"]);
    }
}

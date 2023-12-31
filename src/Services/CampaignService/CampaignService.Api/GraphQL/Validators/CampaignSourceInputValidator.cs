using CampaignService.Api.GraphQL.Types.Inputs;
using FluentValidation;

namespace CampaignService.Api.GraphQL.Validators;

public class CampaignSourceInputValidator : AbstractValidator<CampaignSourceInput>
{
    public CampaignSourceInputValidator()
    {
        RuleFor(c => c.EntityId)
            .NotEmpty().NotNull().WithMessage("Entity id cannot be empty");

        RuleFor(c => c.CampaignId)
            .NotEmpty().NotNull().WithMessage("Campaign id cannot be empty");
    }
}

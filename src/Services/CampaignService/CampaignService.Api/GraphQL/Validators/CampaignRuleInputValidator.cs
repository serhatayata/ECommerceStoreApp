using CampaignService.Api.GraphQL.Types.Inputs;
using FluentValidation;

namespace CampaignService.Api.GraphQL.Validators;

public class CampaignRuleInputValidator : AbstractValidator<CampaignRuleInput>
{
    public CampaignRuleInputValidator()
    {
        RuleFor(c => c.CampaignId)
            .NotNull().NotEmpty().WithMessage("Campaign cannot be empty");

        RuleFor(c => c.Type)
            .IsInEnum().WithMessage("Type is not valid");

        RuleFor(c => c.Data)
            .NotNull().NotEmpty().WithMessage("Data cannot be empty")
            .Length(1, 10000).WithMessage("Data length must be between 1-10000");

        RuleFor(c => c.Value)
            .NotNull().NotEmpty().WithMessage("Value cannot be empty")
            .Length(1, 10000).WithMessage("Value length must be between 1-10000");
    }
}

using CampaignService.Api.GraphQL.Types.Inputs;
using FluentValidation;

namespace CampaignService.Api.GraphQL.Validators;

public class CampaignItemInputValidator : AbstractValidator<CampaignItemInput>
{
    public CampaignItemInputValidator()
    {
        RuleFor(c => c.CampaignId)
            .NotNull().NotEmpty().WithMessage("Campaign cannot be empty");

        RuleFor(c => c.UserId)
            .Length(1, 40).WithMessage("User id length must be between 1-40");

        RuleFor(c => c.Description)
            .NotNull().NotEmpty().WithMessage("Description cannot be empty")
            .Length(1, 40).WithMessage("Description length must be between 1-1000");

        RuleFor(c => c.Status)
            .IsInEnum().WithMessage("Status is not valid");

        RuleFor(c => c.ExpirationDate)
            .NotNull().NotEmpty().WithMessage("Expiration date cannot be empty");
    }
}

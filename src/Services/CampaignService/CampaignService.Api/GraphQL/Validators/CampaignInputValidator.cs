using CampaignService.Api.GraphQL.Types.Inputs;
using FluentValidation;

namespace CampaignService.Api.GraphQL.Validators;

public class CampaignInputValidator : AbstractValidator<CampaignInput>
{
    public CampaignInputValidator()
    {
        RuleFor(c => c.Status)
            .NotNull().NotEmpty().WithMessage("Status cannot be empty");

        RuleFor(c => c.Name)
            .NotNull().NotEmpty().WithMessage("Name cannot be empty")
            .Length(1,255).WithMessage("Name length must be between 1-255");

        RuleFor(c => c.Description)
            .NotNull().NotEmpty().WithMessage("Description cannot be empty")
            .Length(1,1000).WithMessage("Description length must be between 1-1000");

        RuleFor(c => c.ExpirationDate)
            .NotNull().NotEmpty().WithMessage("Expiration date cannot be empty");

        RuleFor(c => c.StartDate)
            .NotNull().NotEmpty().WithMessage("Start date cannot be empty");

        RuleFor(c => c.Sponsor)
            .NotNull().NotEmpty().WithMessage("Sponsor cannot be empty")
            .Length(1,255).WithMessage("Sponsor length must be between 1-255");

        RuleFor(c => c.DiscountType)
            .IsInEnum().WithMessage("Discount type is not valid");

        RuleFor(c => c.PlatformType)
            .IsInEnum().WithMessage("Platform type is not valid");

        RuleFor(c => c.CalculationType)
            .IsInEnum().WithMessage("Calculation type is not valid");

        RuleFor(c => c.CalculationAmount)
            .PrecisionScale(8, 2, true).WithMessage("Calculation amount value is not valid scale must be 8,2");

        RuleFor(c => c.Amount)
            .PrecisionScale(8, 2, true).WithMessage("Amount value is not valid scale must be 8,2");

        RuleFor(c => c.MaxUsage)
            .NotNull().NotEmpty().WithMessage("Max usage cannot be empty");

        RuleFor(c => c.MaxUsagePerUser)
            .NotNull().NotEmpty().WithMessage("Max usage per user cannot be empty");
    }
}

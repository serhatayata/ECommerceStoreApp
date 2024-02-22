using CampaignService.Api.GraphQL.Types.Inputs;
using FluentValidation;

namespace CampaignService.Api.GraphQL.Validators;

public class CouponUsageInputValidator : AbstractValidator<CouponUsageInput>
{
    public CouponUsageInputValidator()
    {
        RuleFor(c => c.Code)
            .NotNull().NotEmpty().WithMessage("Code cannot be empty")
            .Length(1, 40).WithMessage("Code length must be between 1-40");

        RuleFor(c => c.UserId)
            .NotNull().NotEmpty().WithMessage("User id cannot be empty")
            .Length(1, 40).WithMessage("User id length must be between 1-40");
    }
}

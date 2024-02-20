using CampaignService.Api.GraphQL.Types.Inputs;
using FluentValidation;

namespace CampaignService.Api.GraphQL.Validators;

public class CouponItemInputValidator : AbstractValidator<CouponItemInput>
{
    public CouponItemInputValidator()
    {
        RuleFor(c => c.CouponId)
            .NotEmpty().NotNull().WithMessage("Coupon id is not valid");

        RuleFor(c => c.Status)
            .IsInEnum().WithMessage("Status is not valid");
    }
}

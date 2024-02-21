using AutoMapper;
using CampaignService.Api.Entities;
using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.Types;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.GraphQL.Types.InputTypes;
using CampaignService.Api.Models.Coupon;
using CampaignService.Api.Repository.Abstract;
using GraphQL;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Mutations;

public class CouponMutation : ObjectGraphType<Coupon>
{
    public CouponMutation(
        ICouponRepository couponRepository,
        IMapper mapper)
    {
        Field<CouponType>("createCoupon")
            .Arguments(new QueryArgument<NonNullGraphType<CouponInputType>> { Name = "coupon" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<Coupon, CouponInput>("coupon");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                    .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var coupon = mapper.Map<Coupon>(validationResult.Model);
                var code = DataGenerationExtensions.RandomCode(8);
                coupon.Code = code;
                var result = await couponRepository.CreateAsync(coupon);
                return result;
            });

        Field<CouponType>("updateCoupon")
            .Arguments(
                new QueryArgument<NonNullGraphType<CouponInputType>> { Name = "coupon" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<Coupon, CouponInput>("coupon");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                                                 .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var coupon = mapper.Map<Coupon>(validationResult.Model);
                return await couponRepository.UpdateAsync(coupon);
            });

        Field<BooleanGraphType>("deleteCoupon")
            .Arguments(
                new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" })
            .ResolveAsync(async (context) =>
            {
                var id = context.GetArgument<int>("id");
                if (id == default)
                {
                    context.Errors.Add(new ExecutionError("Couldn't find parameters for delete"));
                    return false;
                }

                return await couponRepository.DeleteAsync(id);
            });

        Field<CouponUsageType>("couponUsage")
            .Arguments(new QueryArgument<NonNullGraphType<CouponUsageInputType>> { Name = "coupon" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<Coupon, CouponUsageInput>("coupon");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                    .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var couponUsage = mapper.Map<CouponUsage>(validationResult.Model);
                var result = await couponRepository.CouponUsageAsync(couponUsage);
                return result;
            });
    }
}

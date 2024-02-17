using AutoMapper;
using CampaignService.Api.Entities;
using CampaignService.Api.Extensions;
using CampaignService.Api.GraphQL.Types;
using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.GraphQL.Types.InputTypes;
using CampaignService.Api.Repository.Abstract;
using GraphQL;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Mutations;

public class CouponItemMutation : ObjectGraphType<CouponItem>
{
    public CouponItemMutation(
        ICouponItemRepository couponItemRepository,
        IMapper mapper)
    {
        Field<CouponItemType>("createCouponItem")
            .Arguments(new QueryArgument<NonNullGraphType<CouponItemInputType>> { Name = "couponItem" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<CouponItem, CouponItemInput>("couponItem");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                    .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var couponItem = mapper.Map<CouponItem>(validationResult.Model);
                var result = await couponItemRepository.CreateAsync(couponItem);
                return result;
            });

        Field<CouponItemType>("updateCouponItem")
            .Arguments(
                new QueryArgument<NonNullGraphType<CouponItemInputType>> { Name = "couponItem" })
            .ResolveAsync(async (context) =>
            {
                var validationResult = context.GetValidationResult<CouponItem, CouponItemInput>("couponItem");
                if (!validationResult.IsSuccess)
                {
                    var errors = validationResult.ErrorMessages
                                                 .Select(s => new ExecutionError(s));

                    context.Errors.AddRange(errors);
                    return null;
                }

                var couponItem = mapper.Map<CouponItem>(validationResult.Model);
                return await couponItemRepository.UpdateAsync(couponItem);
            });

        Field<BooleanGraphType>("deleteCouponItem")
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

                return await couponItemRepository.DeleteAsync(id);
            });
    }
}

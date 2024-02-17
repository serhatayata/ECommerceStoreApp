﻿using CampaignService.Api.Entities;
using CampaignService.Api.GraphQL.Types.Enums;
using GraphQL.Types;

namespace CampaignService.Api.GraphQL.Types;

public class CouponItemType : ObjectGraphType<CouponItem>
{
    public CouponItemType()
    {
        Name = "couponItemInput";
        Field<IntGraphType>("id");
        Field<NonNullGraphType<IntGraphType>>("couponId");
        Field<StringGraphType>("userId");
        Field<NonNullGraphType<StringGraphType>>("code");
        Field<NonNullGraphType<CouponItemStatusEnumType>>("status");
        Field<IntGraphType>("orderId");
    }
}

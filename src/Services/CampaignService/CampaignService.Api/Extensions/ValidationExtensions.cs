using CampaignService.Api.GraphQL.Types.Inputs;
using CampaignService.Api.Models;
using GraphQL;

namespace CampaignService.Api.Extensions;

public static class ValidationExtensions
{
    public static ValidationModel<R> GetValidationResult<T, R>(this IResolveFieldContext<T> context, string name)
    {
		try
		{
            var input = context.GetValidatedArgument<R>(name);
            return new ValidationModel<R>(input);
        }
		catch (FluentValidation.ValidationException ex)
		{
            var errorMessages = ex.Errors
                                  .Select(s => s.ErrorMessage)
                                  .ToArray();

            return new ValidationModel<R>(errorMessages);
		}
    }
}

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using BasketService.Api.Infrastructure.Filters;

namespace BasketService.Api.Infrastructure.Attributes;

public class FluentValidationCustomValidationAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .ToArray();

            var responseObj = new JsonErrorResponse()
            {
                Messages = errors
            };

            context.Result = new JsonResult(responseObj)
            {
                StatusCode = 400
            };
        }
    }
}

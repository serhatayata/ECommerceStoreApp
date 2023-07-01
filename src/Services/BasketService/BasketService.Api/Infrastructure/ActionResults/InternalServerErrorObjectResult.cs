using Microsoft.AspNetCore.Mvc;

namespace BasketService.Api.Infrastructure.ActionResults;
public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object error)
        : base(error)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}


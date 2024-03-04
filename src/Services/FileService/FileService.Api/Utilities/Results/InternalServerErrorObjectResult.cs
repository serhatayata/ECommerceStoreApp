using Microsoft.AspNetCore.Mvc;

namespace FileService.Api.Utilities.Results;

public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object error)
    : base(error)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}


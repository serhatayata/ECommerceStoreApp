using Microsoft.AspNetCore.Mvc;

namespace Monitoring.BackgroundTasks.Utilities.Results;

public class InternalServerErrorObjectResult : ObjectResult
{
    public InternalServerErrorObjectResult(object error)
: base(error)
    {
        StatusCode = StatusCodes.Status500InternalServerError;
    }
}

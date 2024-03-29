﻿using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Utilities.Results
{
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object error)
    : base(error)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}

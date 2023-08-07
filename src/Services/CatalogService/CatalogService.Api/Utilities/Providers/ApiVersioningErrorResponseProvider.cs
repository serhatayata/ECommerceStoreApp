using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

namespace CatalogService.Api.Utilities.Providers
{
    public class ApiVersioningErrorResponseProvider : DefaultErrorResponseProvider
    {
        public override IActionResult CreateResponse(ErrorResponseContext context)
        {
            string message = "Something went wrong while selecting the api version";
            var errorResponse = new ErrorResult(message, context.StatusCode);

            var response = new ObjectResult(errorResponse);
            response.StatusCode = context.StatusCode;

            return response;
        }
    }
}

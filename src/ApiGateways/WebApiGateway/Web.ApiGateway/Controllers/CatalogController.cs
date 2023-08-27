using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Web.ApiGateway.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableCors("CorsPolicy")]
public class CatalogController : ControllerBase
{

}

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public class BaseApiController : ControllerBase
{
}

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmeriCorps.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public sealed class ProjectController(IProjectControllerService service) : ControllerBase
{
    private readonly IProjectControllerService _service = service;


    [HttpGet("{projCode}")]
    public async Task<IActionResult> GetProjectByCode(string projCode) =>
        await ServeAsync(async () => await _service.GetProjectByCode(projCode));

    [HttpPost("create")]
    public async Task<IActionResult> CreateProject([FromBody] ProjectRequestModel projRequest) =>
        await ServeAsync(async () => await _service.CreateProject(projRequest));






    private async Task<IActionResult> ServeAsync<T>(Func<Task<(ResponseStatus, T)>> callAsync)
    {
        var (status, response) = await callAsync();
        return status switch
        {
            ResponseStatus.MissingInformation => new StatusCodeResult((int)HttpStatusCode.UnprocessableContent),
            ResponseStatus.UnknownError => new StatusCodeResult((int)HttpStatusCode.InternalServerError),
            ResponseStatus.Successful => new OkObjectResult(response),
            _ => Ok()
        };
    }
}
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmeriCorps.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public sealed class AccessController(IAccessControllerService service) : ControllerBase
{
    private readonly IAccessControllerService _service = service;

    [HttpGet("get/{accessName}")]
    public async Task<IActionResult> GetAccessByNameAsync(string accessName) =>
        await ServeAsync(async () => await _service.GetAccessByNameAsync(accessName));

    [HttpGet("list-all")]
    public async Task<IActionResult> GetAccessListAsync() =>
        await ServeAsync(async () => await _service.GetAccessListAsync());

    [HttpGet("list/{accessType}")]
    public async Task<IActionResult> GetAccessListByTypeAsync(string accessType) =>
        await ServeAsync(async () => await _service.GetAccessListByTypeAsync(accessType));

    [HttpPost("create")]
    public async Task<IActionResult> CreateAccessAsync([FromBody] AccessRequestModel accessRequest) =>
        await ServeAsync(async () => await _service.CreateAccessAsync(accessRequest));

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
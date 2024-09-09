using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmeriCorps.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public sealed class OrgController(IOrgControllerService service) : ControllerBase
{
    private readonly IOrgControllerService _service = service;


    [HttpGet("{orgCode}")]
    public async Task<IActionResult> GetOrgByCodeAsync(string orgCode) =>
        await ServeAsync(async () => await _service.GetOrgByCodeAsync(orgCode));

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrgAsync([FromBody] OrganizationRequestModel orgRequest) =>
        await ServeAsync(async () => await _service.CreateOrgAsync(orgRequest));






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
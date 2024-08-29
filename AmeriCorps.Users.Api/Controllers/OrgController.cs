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
    public async Task<IActionResult> GetOrgByCode(string orgCode) =>
        await ServeAsync(async () => await _service.GetOrgByCode(orgCode));

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrg([FromBody] OrganizationRequestModel orgRequest) =>
        await ServeAsync(async () => await _service.CreateOrg(orgRequest));






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
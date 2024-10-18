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
    public async Task<IActionResult> GetProjectByCodeAsync(string projCode) =>
        await ServeAsync(async () => await _service.GetProjectByCodeAsync(projCode));

    [HttpPost("create")]
    public async Task<IActionResult> CreateProjectAsync([FromBody] ProjectRequestModel projRequest) =>
        await ServeAsync(async () => await _service.CreateProjectAsync(projRequest));

    [HttpGet("list/{orgCode}")]
    public async Task<IActionResult> GetProjectListByOrgAsync(string orgCode) =>
        await ServeAsync(async () => await _service.GetProjectListByOrgAsync(orgCode));

    [HttpPost("update")]
    public async Task<IActionResult> UpdateProjectAsync([FromBody] ProjectRequestModel projRequest) =>
        await ServeAsync(async () => await _service.UpdateProjectAsync(projRequest));

    [HttpPost("op-site/update")]
    public async Task<IActionResult> UpdateOperatingSiteAsync([FromBody] OperatingSiteRequestModel opSiteRequest) =>
        await ServeAsync(async () => await _service.UpdateOperatingSiteAsync(opSiteRequest));

    [HttpPost("op-site/invite")]
    public async Task<IActionResult> InviteOperatingSiteAsync([FromBody] OperatingSiteRequestModel opSiteRequest) =>
        await ServeAsync(async () => await _service.InviteOperatingSiteAsync(opSiteRequest));

    [HttpPost("search")]
    public async Task<IActionResult> SearchProjectsAsync([FromBody] SearchFiltersRequestModel filters) =>
        await ServeAsync(async () => await _service.SearchProjectsAsync(filters));

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
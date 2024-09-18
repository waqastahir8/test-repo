using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AmeriCorps.Users.Models;


namespace AmeriCorps.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public sealed class RolesController(IRolesControllerService service) : ControllerBase
{
    private readonly IRolesControllerService _service = service;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoleAsync(int id) =>
      await ServeAsync(async () => await _service.GetAsync(id));


    [HttpPost]
    public async Task<IActionResult> CreateRoleAsync([FromBody] RoleRequestModel roleRequest) =>
        await ServeAsync(async () => await _service.CreateOrPatchAsync(roleRequest));


    [HttpPatch]
    public async Task<IActionResult> PatchUserAsync([FromBody] RoleRequestModel roleRequest) =>
        await ServeAsync(async () => await _service.CreateOrPatchAsync(roleRequest));

    [HttpGet("list/{roleType}")]
    public async Task<IActionResult> GetRoleListByTypeAsync(string roleType) =>
        await ServeAsync(async () => await _service.GetRoleListByTypeAsync(roleType));



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
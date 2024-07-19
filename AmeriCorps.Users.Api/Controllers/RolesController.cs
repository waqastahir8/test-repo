using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.ControllerServices;


namespace AmeriCorps.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public sealed class RolesController(IRolesControllerService service) : ControllerBase
{
    private readonly IRolesControllerService _service = service;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserAsync(int id) =>
      await ServeAsync(async () => await _service.GetAsync(id));


    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] RoleRequestModel userRequest) =>
        await ServeAsync(async () => await _service.CreateOrPatchAsync(userRequest));


    [HttpPatch]
    public async Task<IActionResult> PatchUserAsync([FromBody] UserRequestModel userRequest) =>
        await ServeAsync(async () => await _service.CreateOrPatchAsync(userRequest));



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

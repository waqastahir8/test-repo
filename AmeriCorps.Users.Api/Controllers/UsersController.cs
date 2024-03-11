using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AmeriCorps.Users.Api.Models;


namespace AmeriCorps.Users.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public sealed class UsersController(IUsersControllerService service) : ControllerBase
{
    private readonly IUsersControllerService _service = service;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserAsync(int id) =>
        await  ServeAsync(async () => await _service.GetAsync(id));
    

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserDTO userRequest) =>
        await ServeAsync(async () => await _service.CreateAsync(userRequest));


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
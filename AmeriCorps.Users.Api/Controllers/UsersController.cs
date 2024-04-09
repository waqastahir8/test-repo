using System.Net;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using AmeriCorps.Users.Models;


namespace AmeriCorps.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public sealed class UsersController(IUsersControllerService service) : ControllerBase
{
    private readonly IUsersControllerService _service = service;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserAsync(int id) =>
        await ServeAsync(async () => await _service.GetAsync(id));

    [HttpGet]
    public async Task<IActionResult> GetUserByExternalAccountId([FromQuery] string externalAccountId) =>
        await ServeAsync(async() => await _service.GetByExternalAccountId(externalAccountId));

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserRequestModel userRequest) =>
        await ServeAsync(async () => await _service.CreateOrPatchAsync(userRequest));

    [HttpPatch]
    public async Task<IActionResult> PatchUserAsync([FromBody] UserRequestModel userRequest) =>
        await ServeAsync(async () => await _service.CreateOrPatchAsync(userRequest));

    [HttpGet("{userId}/Searches")]
    public async Task<IActionResult> GetUserSearchesAsync(int userId) =>
        await ServeAsync(async () => await _service.GetUserSearchesAsync(userId));

    [HttpPost("{userId}/Searches")]
    public async Task<IActionResult> CreateSearchAsync(int userId, [FromBody] SavedSearchRequestModel searchRequest) =>
        await ServeAsync(async () => await _service.CreateSearchAsync(userId, searchRequest));

    [HttpPut("{userId}/Searches/{searchId}")]
    public async Task<IActionResult> CreateSearchAsync(int userId, int searchId, [FromBody] SavedSearchRequestModel searchRequest) =>
       await ServeAsync(async () => await _service.UpdateSearchAsync(userId, searchId, searchRequest));

    [HttpDelete("{userId}/Searches/{searchId}")]
    public async Task<IActionResult> DeleteSearchAsync(int userId, int searchId) =>
        await ServeAsync(async () => await _service.DeleteSearchAsync(userId, searchId));
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
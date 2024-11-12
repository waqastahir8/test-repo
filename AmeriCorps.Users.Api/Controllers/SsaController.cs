using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmeriCorps.Users.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiVersion("1.0")]
public sealed class SsaController(ISsaControllerService service) : ControllerBase
{
    private readonly ISsaControllerService _service = service;

    [HttpPost("bulk-update")]
    public async Task<IActionResult> BulkUpdateVerificationDataAsync([FromBody] List<SocialSecurityVerificationRequestModel> updateList) =>
        await ServeAsync(async () => await _service.BulkUpdateVerificationDataAsync(updateList));

    //Update User's SSA Verification Info
    [HttpPost("SSA/update/{userId}")]
    public async Task<IActionResult> UpdateUserSSAInfo(int userId, [FromBody] SocialSecurityVerificationRequestModel verificationUpdate) =>
        await ServeAsync(async () => await _service.UpdateUserSSAInfo(userId, verificationUpdate));

    [HttpGet("submit/{userId}")]
    public async Task<IActionResult> SubmitInfoForVerificationAsync(int userId) =>
        await ServeAsync(async () => await _service.SubmitInfoForVerificationAsync(userId));

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
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        await ServeAsync(async () => await _service.GetByExternalAccountIdAsync(externalAccountId));

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] UserRequestModel userRequest) =>
        await ServeAsync(async () => await _service.CreateOrPatchAsync(userRequest));

    [HttpPatch]
    public async Task<IActionResult> PatchUserAsync([FromBody] UserRequestModel userRequest) =>
        await ServeAsync(async () => await _service.CreateOrPatchAsync(userRequest));

    [HttpGet("{userId}/Searches")]
    public async Task<IActionResult> GetUserSearchesAsync(int userId) =>
        await ServeAsync(async () => await _service.GetUserSearchesAsync(userId));

    [HttpGet("Attributes/{type}/{value}")]
    public async Task<IActionResult> GetByAttributeAsync(string type, string value) =>
        await ServeAsync(async () => await _service.GetAsync(attributeType: type, attributeValue: value));

    [HttpPost("{userId}/Searches")]
    public async Task<IActionResult> CreateSearchAsync(int userId, [FromBody] SavedSearchRequestModel? searchRequest) =>
        await ServeAsync(async () => await _service.CreateSearchAsync(userId, searchRequest));

    [HttpPut("{userId}/Searches/{searchId}")]
    public async Task<IActionResult> UpdateSearchAsync(int userId, int searchId, [FromBody] SavedSearchRequestModel? searchRequest) =>
       await ServeAsync(async () => await _service.UpdateSearchAsync(userId, searchId, searchRequest));

    [HttpDelete("{userId}/Searches/{searchId}")]
    public async Task<IActionResult> DeleteSearchAsync(int userId, int searchId) =>
        await ServeAsync(async () => await _service.DeleteSearchAsync(userId, searchId));

    [HttpGet("{userId}/Reference")]
    public async Task<IActionResult> GetUserReferencesAsync(int userId) =>
        await ServeAsync(async () => await _service.GetReferencesAsync(userId));

    [HttpPost("{userId}/References")]
    public async Task<IActionResult> CreateReferenceAsync(int userId, [FromBody] ReferenceRequestModel? referenceRequest) =>
        await ServeAsync(async () => await _service.CreateReferenceAsync(userId, referenceRequest));

    [HttpPut("{userId}/References/{referenceId}")]
    public async Task<IActionResult> UpdateReferenceAsync(int userId, int referenceId, [FromBody] ReferenceRequestModel? referenceRequest) =>
       await ServeAsync(async () => await _service.UpdateReferenceAsync(userId, referenceId, referenceRequest));

    [HttpDelete("{userId}/References/{referenceId}")]
    public async Task<IActionResult> DeleteReferenceAsync(int userId, int referenceId) =>
        await ServeAsync(async () => await _service.DeleteReferenceAsync(userId, referenceId));

    [HttpPost("Collection")]
    public async Task<IActionResult> CreateCollectionAsync([FromBody] CollectionRequestModel collectionRequest) =>
        await ServeAsync(async () => await _service.CreateCollectionAsync(collectionRequest));

    [HttpGet("Collection/{userId}/{type}")]
    public async Task<IActionResult> GetUserCollectionsAsync(int userId, string? type) =>
        await ServeAsync(async () => await _service.GetCollectionAsync(userId, type));

    [HttpDelete("Collection")]
    public async Task<IActionResult> DeleteUserCollectionsAsync([FromBody] CollectionListRequestModel? requestModel) =>
        await ServeAsync(async () => await _service.DeleteCollectionAsync(requestModel));

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

    //Associate a role with a user by passing userId and RoleID
    [HttpPost("append/{userId}/role/{roleId}")]
    public async Task<IActionResult> AssociateRoleAsync(int userId, int roleId) =>
        await ServeAsync(async () => await _service.AssociateRoleAsync(userId, roleId));

    //Add some roles to a user by passing userId
    [HttpPost("append/{userId}/roles")]
    public async Task<IActionResult> AddRoleToUserAsync(int userId, [FromBody] RoleRequestModel roleRequest) =>
        await ServeAsync(async () => await _service.AddRoleToUserAsync(userId, roleRequest));

    //Associate a role with a user by passing userId and RoleID
    [HttpGet("project/add/{userId}/{projCode}")]
    public async Task<IActionResult> AddUserToProjectAsync(int userId, string projCode) =>
        await ServeAsync(async () => await _service.AddUserToProjectAsync(userId, projCode));

    //Fetch List of Users by Org Code
    [HttpGet("org/users/{orgCode}")]
    public async Task<IActionResult> FetchUserListByOrgCodeAsync(String orgCode) =>
        await ServeAsync(async () => await _service.FetchUserListByOrgCodeAsync(orgCode));

    //Update User acces, roles and projects
    [HttpPost("org/users/update")]
    public async Task<IActionResult> UpdateUserProjectAndRoleDataAsync([FromBody] UserProjectRoleUpdateRequestModel toUpdate) =>
        await ServeAsync(async () => await _service.UpdateUserProjectAndRoleDataAsync(toUpdate));

    //Invite user to org
    [HttpPost("org/users/invite")]
    public async Task<IActionResult> InviteUserAsync([FromBody] UserRequestModel toInvite) =>
        await ServeAsync(async () => await _service.InviteUserAsync(toInvite));
}
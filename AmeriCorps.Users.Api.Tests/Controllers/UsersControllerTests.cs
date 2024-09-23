using AmeriCorps.Users.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class UsersControllerTests : BaseTests<UsersController>
{
    private Mock<IUsersControllerService>? _serviceMock;

    [Fact]
    public async Task GetUserAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var userResponse = Fixture.Create<UserResponse>();

        _serviceMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync((ResponseStatus.Successful, userResponse));
        //Act
        var actual = await sut.GetUserAsync(userId);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUserAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();

        _serviceMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.GetUserAsync(userId);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task GetUserByExternalAccountId_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var externalAccountId = Fixture.Create<string>();
        var userResponse = Fixture.Create<UserResponse>();

        _serviceMock!
            .Setup(x => x.GetByExternalAccountIdAsync(externalAccountId))
            .ReturnsAsync((ResponseStatus.Successful, userResponse));

        //Act
        var actual = await sut.GetUserByExternalAccountId(externalAccountId);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUserByExternalAccountId_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var externalAccountId = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetByExternalAccountIdAsync(externalAccountId))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.GetUserByExternalAccountId(externalAccountId);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task CreateUserAsync_InformationMissing_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<UserRequestModel>();

        _serviceMock!
            .Setup(x => x.CreateOrPatchAsync(model))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));

        //Act
        var actual = await sut.CreateUserAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task CreateUserAsync_UnknownError_500StatusCode()
    {

        //Arrange
        var sut = Setup();
        var model = Fixture.Create<UserRequestModel>();

        _serviceMock!
            .Setup(x => x.CreateOrPatchAsync(model))
            .ReturnsAsync((ResponseStatus.UnknownError, null));

        //Act
        var actual = await sut.CreateUserAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task CreateUserAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<UserRequestModel>();
        _serviceMock!
            .Setup(x => x.CreateOrPatchAsync(model))
            .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<UserResponse>()));

        // Act
        var actual = await sut.CreateUserAsync(model);

        // Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    /// 


    [Fact]
    public async Task PatchUserAsync_InformationMissing_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<UserRequestModel>();

        _serviceMock!
            .Setup(x => x.CreateOrPatchAsync(model))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));

        //Act
        var actual = await sut.PatchUserAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task PatchUserAsync_UnknownError_500StatusCode()
    {

        //Arrange
        var sut = Setup();
        var model = Fixture.Create<UserRequestModel>();

        _serviceMock!
            .Setup(x => x.CreateOrPatchAsync(model))
            .ReturnsAsync((ResponseStatus.UnknownError, null));

        //Act
        var actual = await sut.PatchUserAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task PatchUserAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<UserRequestModel>();
        _serviceMock!
            .Setup(x => x.CreateOrPatchAsync(model))
            .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<UserResponse>()));

        // Act
        var actual = await sut.PatchUserAsync(model);

        // Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateSearchAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var model = Fixture.Create<SavedSearchRequestModel?>();
        _serviceMock!
            .Setup(x => x.CreateSearchAsync(userId, model))
            .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<SavedSearchResponseModel>()));

        // Act
        var actual = await sut.CreateSearchAsync(userId, model);

        // Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUserSearchesAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();

        _serviceMock!
            .Setup(x => x.GetUserSearchesAsync(userId))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.GetUserSearchesAsync(userId);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task UpdateSearchAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();

        var model = Fixture.Create<SavedSearchRequestModel?>();
        _serviceMock!
            .Setup(x => x.UpdateSearchAsync(userId, referenceId, model))
            .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<SavedSearchResponseModel>()));

        // Act
        var actual = await sut.UpdateSearchAsync(userId, referenceId, model);

        // Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateSearchAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();

        var model = Fixture.Create<SavedSearchRequestModel?>();
        _serviceMock!
            .Setup(x => x.UpdateSearchAsync(userId, searchId, model))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));

        // Act
        var actual = await sut.UpdateSearchAsync(userId, searchId, model);

        // Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task UpdateSearchAsync_UnknownError_500StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();

        var model = Fixture.Create<SavedSearchRequestModel?>();
        _serviceMock!
            .Setup(x => x.UpdateSearchAsync(userId, searchId, model))
            .ReturnsAsync((ResponseStatus.UnknownError, null));

        // Act
        var actual = await sut.UpdateSearchAsync(userId, searchId, model);

        // Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task DeleteSearchAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var searchId = Fixture.Create<int>();

        var model = Fixture.Create<SavedSearchRequestModel?>();
        _serviceMock!
            .Setup(x => x.DeleteSearchAsync(userId, searchId))
            .ReturnsAsync((ResponseStatus.Successful, true));

        // Act
        var actual = await sut.DeleteSearchAsync(userId, searchId);

        // Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateReferenceAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var model = Fixture.Create<ReferenceRequestModel?>();
        _serviceMock!
            .Setup(x => x.CreateReferenceAsync(userId, model))
            .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<ReferenceResponseModel>()));

        // Act
        var actual = await sut.CreateReferenceAsync(userId, model);

        // Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetUserReferencesAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();

        _serviceMock!
            .Setup(x => x.GetReferencesAsync(userId))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.GetUserReferencesAsync(userId);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task GetUserReferencesAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();

        _serviceMock!
            .Setup(x => x.GetReferencesAsync(userId))
            .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<UserReferencesResponseModel>()));

        //Act
        var actual = await sut.GetUserReferencesAsync(userId);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReferenceAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();

        var model = Fixture.Create<ReferenceRequestModel?>();
        _serviceMock!
            .Setup(x => x.UpdateReferenceAsync(userId, referenceId, model))
            .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<ReferenceResponseModel>()));

        // Act
        var actual = await sut.UpdateReferenceAsync(userId, referenceId, model);

        // Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReferenceAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();

        var model = Fixture.Create<ReferenceRequestModel?>();
        _serviceMock!
            .Setup(x => x.UpdateReferenceAsync(userId, referenceId, model))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));

        // Act
        var actual = await sut.UpdateReferenceAsync(userId, referenceId, model);

        // Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task UpdateReferenceAsync_UnknownError_500StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();

        var model = Fixture.Create<ReferenceRequestModel?>();
        _serviceMock!
            .Setup(x => x.UpdateReferenceAsync(userId, referenceId, model))
            .ReturnsAsync((ResponseStatus.UnknownError, null));

        // Act
        var actual = await sut.UpdateReferenceAsync(userId, referenceId, model);

        // Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task DeleteReferenceAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var referenceId = Fixture.Create<int>();

        var model = Fixture.Create<ReferenceRequestModel?>();
        _serviceMock!
            .Setup(x => x.DeleteReferenceAsync(userId, referenceId))
            .ReturnsAsync((ResponseStatus.Successful, true));

        // Act
        var actual = await sut.DeleteReferenceAsync(userId, referenceId);

        // Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task AddUserToProjectAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var projCode = Fixture.Create<string>();
        var userResponse = Fixture.Create<UserResponse>();

        _serviceMock!
            .Setup(x => x.AddUserToProjectAsync(userId,projCode))
            .ReturnsAsync((ResponseStatus.Successful, userResponse));
        //Act
        var actual = await sut.AddUserToProjectAsync(userId,projCode);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AddUserToProjectAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var projCode = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.AddUserToProjectAsync(userId,projCode))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.AddUserToProjectAsync(userId,projCode);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task FetchUserListByOrgCodeAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var orgCode = Fixture.Create<string>();
        var userResponse = Fixture.Create<UserListResponse>();

        _serviceMock!
            .Setup(x => x.FetchUserListByOrgCodeAsync(orgCode))
            .ReturnsAsync((ResponseStatus.Successful, userResponse));
        //Act
        var actual = await sut.FetchUserListByOrgCodeAsync(orgCode);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task FetchUserListByOrgCodeAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var orgCode = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.FetchUserListByOrgCodeAsync(orgCode))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.FetchUserListByOrgCodeAsync(orgCode);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUserProjectAndRoleDataAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var toUpdate = Fixture.Create<UserProjectRoleUpdateRequestModel>();
        var userResponse = Fixture.Create<UserResponse>();

        _serviceMock!
            .Setup(x => x.UpdateUserProjectAndRoleDataAsync(toUpdate))
            .ReturnsAsync((ResponseStatus.Successful, userResponse));
        //Act
        var actual = await sut.UpdateUserProjectAndRoleDataAsync(toUpdate);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUserProjectAndRoleDataAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var toUpdate = Fixture.Create<UserProjectRoleUpdateRequestModel>();

        _serviceMock!
            .Setup(x => x.UpdateUserProjectAndRoleDataAsync(toUpdate))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.UpdateUserProjectAndRoleDataAsync(toUpdate);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }


    [Fact]
    public async Task InviteUserAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var toInvite = Fixture.Create<UserRequestModel>();
        var userResponse = Fixture.Create<UserResponse>();

        _serviceMock!
            .Setup(x => x.InviteUserAsync(toInvite))
            .ReturnsAsync((ResponseStatus.Successful, userResponse));
        //Act
        var actual = await sut.InviteUserAsync(toInvite);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task InviteUserAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var toInvite = Fixture.Create<UserRequestModel>();

        _serviceMock!
            .Setup(x => x.InviteUserAsync(toInvite))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.InviteUserAsync(toInvite);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    protected override UsersController Setup()
    {
        _serviceMock = new();
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(_serviceMock.Object);
    }
}
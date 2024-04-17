using AmeriCorps.Users.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AmeriCorps.Users.Api.Tests;

public sealed class UsersControllerTests : BaseTests<UsersController>
{
    private Mock<IUsersControllerService>? _serviceMock;

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
    public async Task GetUserByExternalAccountId_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var externalAccountId = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetByExternalAccountId(externalAccountId))
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

    protected override UsersController Setup()
    {
        _serviceMock = new();
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(_serviceMock.Object);
    }
}
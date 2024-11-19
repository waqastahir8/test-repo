using AmeriCorps.Users.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class SsaControllerTests : BaseTests<SsaController>
{
    private Mock<ISsaControllerService>? _serviceMock;

    [Fact]
    public async Task BulkUpdateVerificationDataAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var updateList = Fixture.Create<List<SocialSecurityVerificationRequestModel>>();
        var success = Fixture.Create<bool>();

        _serviceMock!
            .Setup(x => x.BulkUpdateVerificationDataAsync(updateList))
            .ReturnsAsync((ResponseStatus.Successful, success));
        //Act
        var actual = await sut.BulkUpdateVerificationDataAsync(updateList);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BulkUpdateVerificationDataAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var updateList = Fixture.Create<List<SocialSecurityVerificationRequestModel>>();

        _serviceMock!
            .Setup(x => x.BulkUpdateVerificationDataAsync(updateList))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.BulkUpdateVerificationDataAsync(updateList);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task BulkUpdateVerificationDataAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var updateList = Fixture.Create<List<SocialSecurityVerificationRequestModel>>();

        _serviceMock!
            .Setup(x => x.BulkUpdateVerificationDataAsync(updateList))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.BulkUpdateVerificationDataAsync(updateList);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUserSSAInfoAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var update = Fixture.Create<SocialSecurityVerificationRequestModel>();
        var updated = Fixture.Create<SocialSecurityVerificationResponse>();

        _serviceMock!
            .Setup(x => x.UpdateUserSSAInfoAsync(update))
            .ReturnsAsync((ResponseStatus.Successful, updated));
        //Act
        var actual = await sut.UpdateUserSSAInfoAsync(update);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateUserSSAInfoAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var update = Fixture.Create<SocialSecurityVerificationRequestModel>();

        _serviceMock!
            .Setup(x => x.UpdateUserSSAInfoAsync(update))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.UpdateUserSSAInfoAsync(update);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }


    [Fact]
    public async Task UpdateUserSSAInfoAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var update = Fixture.Create<SocialSecurityVerificationRequestModel>();

        _serviceMock!
            .Setup(x => x.UpdateUserSSAInfoAsync(update))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.UpdateUserSSAInfoAsync( update);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }


    [Fact]
    public async Task SubmitInfoForVerificationAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();
        var user = Fixture.Create<UserResponse>();

        _serviceMock!
            .Setup(x => x.SubmitInfoForVerificationAsync(userId))
            .ReturnsAsync((ResponseStatus.Successful, user));
        //Act
        var actual = await sut.SubmitInfoForVerificationAsync(userId);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SubmitInfoForVerificationAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();

        _serviceMock!
            .Setup(x => x.SubmitInfoForVerificationAsync(userId))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.SubmitInfoForVerificationAsync(userId);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task SubmitInfoForVerificationAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userId = Fixture.Create<int>();

        _serviceMock!
            .Setup(x => x.SubmitInfoForVerificationAsync(userId))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.SubmitInfoForVerificationAsync(userId);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task FetchPendingUsersForSSAVerificationAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var userList = Fixture.Create<List<UserResponse>>();

        _serviceMock!
            .Setup(x => x.FetchPendingUsersForSSAVerificationAsync())
            .ReturnsAsync((ResponseStatus.Successful, userList));
        //Act
        var actual = await sut.FetchPendingUsersForSSAVerificationAsync();

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task FetchPendingUsersForSSAVerificationAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();

        _serviceMock!
            .Setup(x => x.FetchPendingUsersForSSAVerificationAsync())
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.FetchPendingUsersForSSAVerificationAsync();

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task FetchPendingUsersForSSAVerificationAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();

        _serviceMock!
            .Setup(x => x.FetchPendingUsersForSSAVerificationAsync())
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.FetchPendingUsersForSSAVerificationAsync();

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }
    ////


    [Fact]
    public async Task NotifyFailedUserVerificationsAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var success = Fixture.Create<bool>();

        _serviceMock!
            .Setup(x => x.NotifyFailedUserVerificationsAsync())
            .ReturnsAsync((ResponseStatus.Successful, success));
        //Act
        var actual = await sut.NotifyFailedUserVerificationsAsync();

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task NotifyFailedUserVerificationsAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();

        _serviceMock!
            .Setup(x => x.NotifyFailedUserVerificationsAsync())
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.NotifyFailedUserVerificationsAsync();

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task NotifyFailedUserVerificationsAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();

        _serviceMock!
            .Setup(x => x.NotifyFailedUserVerificationsAsync())
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.NotifyFailedUserVerificationsAsync();

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    protected override SsaController Setup()
    {
        _serviceMock = new();
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(_serviceMock.Object);
    }

}
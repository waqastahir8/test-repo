using AmeriCorps.Users.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class AccessControllerTests : BaseTests<AccessController>
{
    private Mock<IAccessControllerService>? _serviceMock;



    [Fact]
    public async Task GetAccessByNameAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var accessName = Fixture.Create<string>();
        var accessResponse = Fixture.Create<AccessResponse>();

        _serviceMock!
            .Setup(x => x.GetAccessByNameAsync(accessName))
            .ReturnsAsync((ResponseStatus.Successful, accessResponse));
        //Act
        var actual = await sut.GetAccessByNameAsync(accessName);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task GetAccessByNameAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var accessName = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetAccessByNameAsync(accessName))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.GetAccessByNameAsync(accessName);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }


    [Fact]
    public async Task GetAccessListByTypeAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var accessName = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetAccessListByTypeAsync(accessName))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.GetAccessListByTypeAsync(accessName);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }


    [Fact]
    public async Task GetAccessListByTypeAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var accessName = Fixture.Create<string>();
        var accessResponse = Fixture.Create<List<AccessResponse>>();

        _serviceMock!
            .Setup(x => x.GetAccessListByTypeAsync(accessName))
            .ReturnsAsync((ResponseStatus.Successful, accessResponse));
        //Act
        var actual = await sut.GetAccessListByTypeAsync(accessName);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task GetAccessListByTypeAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var accessName = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetAccessListByTypeAsync(accessName))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.GetAccessListByTypeAsync(accessName);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task CreateAccessAsync_InformationMissing_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<AccessResponse>();

        _serviceMock!
            .Setup(x => x.CreateAccessAsync(model))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));

        //Act
        var actual = await sut.CreateAccessAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task CreateAccessAsync_UnknownError_500StatusCode()
    {

        //Arrange
        var sut = Setup();
        var model = Fixture.Create<AccessRequestModel>();

        _serviceMock!
            .Setup(x => x.CreateAccessAsync(model))
            .ReturnsAsync((ResponseStatus.UnknownError, null));

        //Act
        var actual = await sut.CreateAccessAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task CreateAccessAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<AccessRequestModel>();

        _serviceMock!
            .Setup(x => x.CreateAccessAsync(model))
            .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<AccessResponse>()));

        // Act
        var actual = await sut.CreateAccessAsync(model);

        // Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }


    protected override AccessController Setup()
    {
        _serviceMock = new();
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(_serviceMock.Object);
    }
}
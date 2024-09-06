using AmeriCorps.Users.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class ProjectControllerTests : BaseTests<ProjectController>
{
    private Mock<IProjectControllerService>? _serviceMock;

    [Fact]
    public async Task GetProjectByCodeAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var projCode = Fixture.Create<string>();
        var projectReponse = Fixture.Create<ProjectResponse>();

        _serviceMock!
            .Setup(x => x.GetProjectByCodeAsync(projCode))
            .ReturnsAsync((ResponseStatus.Successful, projectReponse));
        //Act
        var actual = await sut.GetProjectByCodeAsync(projCode);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProjectByCodeAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var projCode = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetProjectByCodeAsync(projCode))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.GetProjectByCodeAsync(projCode);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task CreateProjectAsync_InformationMissing_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<ProjectResponse>();

        _serviceMock!
            .Setup(x => x.CreateProjectAsync(model))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));

        //Act
        var actual = await sut.CreateProjectAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task CreateProjectAsync_UnknownError_500StatusCode()
    {

        //Arrange
        var sut = Setup();
        var model = Fixture.Create<ProjectRequestModel>();

        _serviceMock!
            .Setup(x => x.CreateProjectAsync(model))
            .ReturnsAsync((ResponseStatus.UnknownError, null));

        //Act
        var actual = await sut.CreateProjectAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task CreateProjectAsync_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<ProjectRequestModel>();

        _serviceMock!
            .Setup(x => x.CreateProjectAsync(model))
            .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<ProjectResponse>()));

        // Act
        var actual = await sut.CreateProjectAsync(model);

        // Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }
    protected override ProjectController Setup()
    {
        _serviceMock = new();
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(_serviceMock.Object);
    }
}
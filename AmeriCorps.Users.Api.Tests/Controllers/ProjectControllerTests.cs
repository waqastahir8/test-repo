using System.Net;
using AmeriCorps.Users.Controllers;
using Microsoft.AspNetCore.Mvc;

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
        var model = Fixture.Create<ProjectRequestModel>();

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

    [Fact]
    public async Task GetProjectListByOrgAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var org = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetProjectListByOrgAsync(org))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.GetProjectListByOrgAsync(org);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task GetProjectListByOrgAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var org = Fixture.Create<string>();
        var projResponse = Fixture.Create<List<ProjectResponse>>();

        _serviceMock!
            .Setup(x => x.GetProjectListByOrgAsync(org))
            .ReturnsAsync((ResponseStatus.Successful, projResponse));
        //Act
        var actual = await sut.GetProjectListByOrgAsync(org);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetProjectListByOrgAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var org = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetProjectListByOrgAsync(org))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.GetProjectListByOrgAsync(org);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProjectAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<ProjectRequestModel>();
        var projResponse = Fixture.Create<ProjectResponse>();

        _serviceMock!
            .Setup(x => x.UpdateProjectAsync(model))
            .ReturnsAsync((ResponseStatus.Successful, projResponse));

        //Act
        var actual = await sut.UpdateProjectAsync(model);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProjectAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<ProjectRequestModel>();

        _serviceMock!
            .Setup(x => x.UpdateProjectAsync(model))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.UpdateProjectAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProjectAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<ProjectRequestModel>();

        _serviceMock!
            .Setup(x => x.UpdateProjectAsync(model))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.UpdateProjectAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task UpdateOperatingSiteAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<OperatingSiteRequestModel>();
        var projResponse = Fixture.Create<OperatingSiteResponse>();

        _serviceMock!
            .Setup(x => x.UpdateOperatingSiteAsync(model))
            .ReturnsAsync((ResponseStatus.Successful, projResponse));

        //Act
        var actual = await sut.UpdateOperatingSiteAsync(model);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UpdateOperatingSiteAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<OperatingSiteRequestModel>();

        _serviceMock!
            .Setup(x => x.UpdateOperatingSiteAsync(model))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.UpdateOperatingSiteAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task UpdateOperatingSiteAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<OperatingSiteRequestModel>();

        _serviceMock!
            .Setup(x => x.UpdateOperatingSiteAsync(model))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.UpdateOperatingSiteAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task InviteOperatingSiteAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();

        var model = Fixture.Create<ProjectRequestModel>();
        var toInvite = Fixture.Create<OperatingSiteRequestModel>();
        model.OperatingSites.Add(toInvite);

        var projResponse = Fixture.Create<ProjectResponse>();
        

        _serviceMock!
            .Setup(x => x.InviteOperatingSiteAsync(model))
            .ReturnsAsync((ResponseStatus.Successful, projResponse));

        //Act
        var actual = await sut.InviteOperatingSiteAsync(model);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task InviteOperatingSiteAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<ProjectRequestModel>();
        var toInvite = Fixture.Create<OperatingSiteRequestModel>();
        model.OperatingSites.Add(toInvite);

        _serviceMock!
            .Setup(x => x.InviteOperatingSiteAsync(model))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.InviteOperatingSiteAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task InviteOperatingSiteAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<ProjectRequestModel>();
        var toInvite = Fixture.Create<OperatingSiteRequestModel>();
        model.OperatingSites.Add(toInvite);

        _serviceMock!
            .Setup(x => x.InviteOperatingSiteAsync(model))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.InviteOperatingSiteAsync(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task SearchProjectsAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var filters = Fixture.Create<SearchFiltersRequestModel>();

        _serviceMock!
            .Setup(x => x.SearchProjectsAsync(filters))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.SearchProjectsAsync(filters);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task SearchProjectsAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var filters = Fixture.Create<SearchFiltersRequestModel>();
        var projResponse = Fixture.Create<List<ProjectResponse>>();

        _serviceMock!
            .Setup(x => x.SearchProjectsAsync(filters))
            .ReturnsAsync((ResponseStatus.Successful, projResponse));
        //Act
        var actual = await sut.SearchProjectsAsync(filters);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SearchProjectsAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var filters = Fixture.Create<SearchFiltersRequestModel>();

        _serviceMock!
            .Setup(x => x.SearchProjectsAsync(filters))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.SearchProjectsAsync(filters);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }


    [Fact]
    public async Task SearchOperatingSitesAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var filters = Fixture.Create<SearchFiltersRequestModel>();

        _serviceMock!
            .Setup(x => x.SearchOperatingSitesAsync(filters))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.SearchOperatingSitesAsync(filters);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task SearchOperatingSitesAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var filters = Fixture.Create<SearchFiltersRequestModel>();
        var projResponse = Fixture.Create<List<OperatingSiteResponse>>();

        _serviceMock!
            .Setup(x => x.SearchOperatingSitesAsync(filters))
            .ReturnsAsync((ResponseStatus.Successful, projResponse));
        //Act
        var actual = await sut.SearchOperatingSitesAsync(filters);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SearchOperatingSitesAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var filters = Fixture.Create<SearchFiltersRequestModel>();

        _serviceMock!
            .Setup(x => x.SearchOperatingSitesAsync(filters))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.SearchOperatingSitesAsync(filters);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }


    protected override ProjectController Setup()
    {
        _serviceMock = new();
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(_serviceMock.Object);
    }
}
using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class ProjectControllerServiceTests : BaseTests<ProjectControllerService>
{
    private Mock<IProjectRepository>? _repositoryMock;
    private Mock<IRequestMapper>? _requestMapperMock;
    private Mock<IResponseMapper>? _responseMapperMock;
    private Mock<IUserHelperService>? _userHelperService;

    [Theory]
    [InlineData("proj")]
    public async Task GetProjectByCodeAsync_Successful_Status(string projCode)
    {
        // Arrange
        var sut = Setup();

        _repositoryMock!
            .Setup(x => x.GetProjectByCodeAsync(projCode))
            .ReturnsAsync(() => Fixture.Build<Project>()
            .Create());

        // Act
        var (status, _) = await sut.GetProjectByCodeAsync(projCode);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData("proj")]
    public async Task GetProjectByCodeAsync_NonExistent_InformationMissing_Status(string projCode)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetProjectByCodeAsync(projCode))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetProjectByCodeAsync(projCode);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateProjectAsync_NullProject_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();

        //Act
        var (status, _) = await sut.CreateProjectAsync(null);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateProjectAsync_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Build<ProjectRequestModel>()
            .Without(o => o.ProjectCode)
            .Create();

        var project =
            Fixture
            .Build<Project>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(project);

        _repositoryMock!
            .Setup(x => x.GetProjectByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(project);

        // Act
        var (status, _) = await sut.CreateProjectAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task CreateProjectAsync_ReturnsMappedObject()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<ProjectRequestModel>();

        var expected =
            Fixture
            .Create<ProjectResponse>();

        var project =
            Fixture
            .Build<Project>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(project);

        _responseMapperMock!
            .Setup(x => x.Map(project))
            .Returns(expected);

        _repositoryMock!
            .Setup(x => x.GetProjectByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(project);

        // Act
        var (_, actual) = await sut.CreateProjectAsync(model);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("proj")]
    public async Task GetProjectListByOrgAsync_Successful_Status(string orgCode)
    {
        // Arrange
        var sut = Setup();

        _repositoryMock!
            .Setup(x => x.GetProjectListByOrgAsync(orgCode))
            .ReturnsAsync(() => Fixture.Build<List<Project>>()
            .Create());

        // Act
        var (status, _) = await sut.GetProjectListByOrgAsync(orgCode);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData("proj")]
    public async Task GetProjectListByOrgAsync_NonExistent_InformationMissing_Status(string orgCode)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetProjectListByOrgAsync(orgCode))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetProjectListByOrgAsync(orgCode);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task UpdateProjectAsync_NullProject_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();

        //Act
        var (status, _) = await sut.UpdateProjectAsync(null);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);

        var model =
            Fixture
            .Build<ProjectRequestModel>()
            .With(o => o.ProjectCode, "")
            .Create();

        //Act
        var (status2, _) = await sut.UpdateProjectAsync(model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status2);
    }

    [Fact]
    public async Task UpdateProjectAsync_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<ProjectRequestModel>();

        var project =
            Fixture
            .Build<Project>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(project);

        _repositoryMock!
            .Setup(x => x.GetProjectByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(project);

        // Act
        var (status, _) = await sut.UpdateProjectAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task UpdateProjectAsync_ReturnsMappedObject()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<ProjectRequestModel>();

        var expected =
            Fixture
            .Create<ProjectResponse>();

        var project =
            Fixture
            .Build<Project>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(project);

        _responseMapperMock!
            .Setup(x => x.Map(project))
            .Returns(expected);

        _repositoryMock!
            .Setup(x => x.GetProjectByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(project);

        // Act
        var (_, actual) = await sut.UpdateProjectAsync(model);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task UpdateOperatingSiteAsync_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();

        var model =
            Fixture
            .Build<OperatingSiteRequestModel>()
            .Without(o => o.Id)
            .Create();

        //Act
        var (status, _) = await sut.UpdateOperatingSiteAsync(model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);

        var model2 =
            Fixture
            .Build<OperatingSiteRequestModel>()
            .With(o => o.ProjectCode, "")
            .Create();

        //Act
        var (status2, _) = await sut.UpdateOperatingSiteAsync(model2);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status2);
    }

    [Fact]
    public async Task UpdateOperatingSiteAsync_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<OperatingSiteRequestModel>();

        var operatingSite =
            Fixture
            .Build<OperatingSite>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(operatingSite);

        _repositoryMock!
            .Setup(x => x.GetOperatingSiteByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(operatingSite);

        // Act
        var (status, _) = await sut.UpdateOperatingSiteAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task UpdateOperatingSiteAsync_ReturnsMappedObject()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<OperatingSiteRequestModel>();

        var expected =
            Fixture
            .Create<OperatingSiteResponse>();

        var operatingSite =
            Fixture
            .Build<OperatingSite>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(operatingSite);

        _responseMapperMock!
            .Setup(x => x.Map(operatingSite))
            .Returns(expected);

        _repositoryMock!
            .Setup(x => x.GetOperatingSiteByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(operatingSite);

        // Act
        var (_, actual) = await sut.UpdateOperatingSiteAsync(model);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task InviteOperatingSiteAsync_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();

        var model =
            Fixture
            .Build<OperatingSiteRequestModel>()
            .Without(o => o.Id)
            .Create();

        //Act
        var (status, _) = await sut.InviteOperatingSiteAsync(model);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);

        var model2 =
            Fixture
            .Build<OperatingSiteRequestModel>()
            .With(o => o.EmailAddress, "")
            .Create();

        //Act
        var (status2, _) = await sut.InviteOperatingSiteAsync(model2);

        //Assert
        //TODO FIX TEST
        Assert.Equal(ResponseStatus.UnknownError, status2);
    }

    [Fact]
    public async Task InviteOperatingSiteAsync_UnknownErrorStatus()
    {
        //Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<OperatingSiteRequestModel>();

        //Act
        var (status, _) = await sut.InviteOperatingSiteAsync(model);

        //Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    // [Fact]
    // public async Task InviteOperatingSiteAsync_SuccessfulStatus()
    // {
    //     // Arrange
    //     var sut = Setup();

    //     var model =
    //         Fixture
    //         .Create<OperatingSiteRequestModel>();

    //     var operatingSite =
    //         Fixture
    //         .Build<OperatingSite>()
    //         .Create();

    //     _requestMapperMock!
    //         .Setup(x => x.Map(model))
    //         .Returns(operatingSite);

    //     _repositoryMock!
    //         .Setup(x => x.GetOperatingSiteByIdAsync(It.IsAny<int>()))
    //         .ReturnsAsync(operatingSite);

    //     // Act
    //     var (status, _) = await sut.InviteOperatingSiteAsync(model);

    //     // Assert
    //     Assert.Equal(ResponseStatus.Successful, status);

    // }

    // [Fact]
    // public async Task InviteOperatingSiteAsync_ReturnsMappedObject()
    // {
    //     // Arrange
    //     var sut = Setup();

    //     var model =
    //         Fixture
    //         .Create<OperatingSiteRequestModel>();

    //     var expected =
    //         Fixture
    //         .Create<OperatingSiteResponse>();

    //     var operatingSite =
    //         Fixture
    //         .Build<OperatingSite>()
    //         .Create();

    //     _requestMapperMock!
    //         .Setup(x => x.Map(model))
    //         .Returns(operatingSite);

    //     _responseMapperMock!
    //         .Setup(x => x.Map(operatingSite))
    //         .Returns(expected);

    //     _repositoryMock!
    //         .Setup(x => x.GetOperatingSiteByIdAsync(It.IsAny<int>()))
    //         .ReturnsAsync(operatingSite);

    //     // Act
    //     var (_, actual) = await sut.InviteOperatingSiteAsync(model);

    //     // Assert
    //     Assert.Equal(expected, actual);
    // }

    [Theory]
    [InlineData("proj", "org")]
    public async Task SearchProjectsAsync_Successful_Status(string query, string orgCode)
    {
        // Arrange
        var sut = Setup();
        var filters = Fixture.Create<SearchFiltersRequestModel>();
        filters.Query = query;
        filters.Awarded = true;
        filters.Active = true;
        filters.OrgCode = orgCode;

        _repositoryMock!
            .Setup(x => x.SearchAwardedProjectsAsync(filters.Query, filters.Active, filters.OrgCode))
            .ReturnsAsync(() => Fixture.Build<List<Project>>()
            .Create());

        // Act
        var (status, _) = await sut.SearchProjectsAsync(filters);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData("proj", "org")]
    public async Task SearchProjectsAsync_NonExistent_InformationMissing_Status(string query, string orgCode)
    {
        // Arrange
        var sut = Setup();
        var filters = Fixture.Create<SearchFiltersRequestModel>();
        filters.Awarded = true;
        filters.Active = true;

        _repositoryMock!
            .Setup(x => x.SearchAwardedProjectsAsync(filters.Query, filters.Active, filters.OrgCode))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.SearchProjectsAsync(filters);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    protected override ProjectControllerService Setup()
    {
        _repositoryMock = new();
        _requestMapperMock = new();
        _responseMapperMock = new();
        _userHelperService = new();

        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(
            Mock.Of<ILogger<ProjectControllerService>>(),
            _requestMapperMock.Object,
            _responseMapperMock.Object,
            _repositoryMock.Object,
            _userHelperService.Object);
    }
}
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


    [Theory]
    [InlineData("proj")]
    public async Task GetProjectByCode_Successful_Status(string projCode)
    {
        // Arrange
        var sut = Setup();
        
        _repositoryMock!
            .Setup(x => x.GetProjectByCode(projCode))
            .ReturnsAsync(() => Fixture.Build<Project>()
            .Create());

        // Act
        var (status, _) = await sut.GetProjectByCode(projCode);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData("proj")]
    public async Task GetProjectByCode_NonExistent_InformationMissing_Status(string projCode)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetProjectByCode(projCode))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetProjectByCode(projCode);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }


    [Fact]
    public async Task CreateProject_NullProject_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();

        //Act
        var (status, _) = await sut.CreateProject(null);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateProject_SuccessfulStatus()
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
            .Setup(x => x.GetProjectByCode(It.IsAny<string>()))
            .ReturnsAsync(project);

        // Act
        var (status, _) = await sut.CreateProject(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);

    }

    [Fact]
    public async Task CreateProject_ReturnsMappedObject()
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
            .Setup(x => x.GetProjectByCode(It.IsAny<string>()))
            .ReturnsAsync(project);

        // Act
        var (_, actual) = await sut.CreateProject(model);

        // Assert
        Assert.Equal(expected, actual);
    }

    protected override ProjectControllerService Setup()
    {
        _repositoryMock = new();
        _requestMapperMock = new();
        _responseMapperMock = new();

        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(
            Mock.Of<ILogger<ProjectControllerService>>(),
            _requestMapperMock.Object,
            _responseMapperMock.Object,
            _repositoryMock.Object);
    }

}
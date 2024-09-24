using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class AccessControllerServiceTests : BaseTests<AccessControllerService>
{
    private Mock<IAccessRepository>? _repositoryMock;
    private Mock<IRequestMapper>? _requestMapperMock;
    private Mock<IResponseMapper>? _responseMapperMock;

    [Theory]
    [InlineData("name")]
    public async Task GetAccessByNameAsync_Successful_Status(string access)
    {
        // Arrange
        var sut = Setup();

        _repositoryMock!
            .Setup(x => x.GetAccessByNameAsync(access))
            .ReturnsAsync(() => Fixture.Build<Access>()
            .Create());

        // Act
        var (status, _) = await sut.GetAccessByNameAsync(access);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData("name")]
    public async Task GetAccessByNameAsync_NonExistent_InformationMissing_Status(string access)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetAccessByNameAsync(access))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetAccessByNameAsync(access);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Theory]
    [InlineData("type")]
    public async Task GetAccessListByTypeAsync_Successful_Status(string access)
    {
        // Arrange
        var sut = Setup();

        _repositoryMock!
            .Setup(x => x.GetAccessListByTypeAsync(access))
            .ReturnsAsync(() => Fixture.Build<List<Access>>()
            .Create());

        // Act
        var (status, _) = await sut.GetAccessListByTypeAsync(access);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData("type")]
    public async Task GetAccessListByTypeAsync_NonExistent_InformationMissing_Status(string access)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetAccessListByTypeAsync(access))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetAccessListByTypeAsync(access);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task GetAccessListAsync_Successful_Status()
    {
        // Arrange
        var sut = Setup();

        _repositoryMock!
            .Setup(x => x.GetAccessListAsync())
            .ReturnsAsync(() => Fixture.Build<List<Access>>()
            .Create());

        // Act
        var (status, _) = await sut.GetAccessListAsync();

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task GetAccessListAsync_NonExistent_InformationMissing_Status()
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetAccessListAsync())
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetAccessListAsync();

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateAccessAsync_NullOrg_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();

        //Act
        var (status, _) = await sut.CreateAccessAsync(null);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateAccessAsync_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<AccessRequestModel>();

        var access =
            Fixture
            .Build<Access>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(access);

        _repositoryMock!
            .Setup(x => x.GetAccessByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(access);

        // Act
        var (status, _) = await sut.CreateAccessAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task CreateAccessAsync_ReturnsMappedObject()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<AccessRequestModel>();

        var expected =
            Fixture
            .Create<AccessResponse>();

        var access =
            Fixture
            .Build<Access>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(access);

        _responseMapperMock!
            .Setup(x => x.Map(access))
            .Returns(expected);

        _repositoryMock!
            .Setup(x => x.GetAccessByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(access);

        // Act
        var (_, actual) = await sut.CreateAccessAsync(model);

        // Assert
        Assert.Equal(expected, actual);
    }

    protected override AccessControllerService Setup()
    {
        _repositoryMock = new();
        _requestMapperMock = new();
        _responseMapperMock = new();

        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(
            Mock.Of<ILogger<AccessControllerService>>(),
            _requestMapperMock.Object,
            _responseMapperMock.Object,
            _repositoryMock.Object);
    }
}
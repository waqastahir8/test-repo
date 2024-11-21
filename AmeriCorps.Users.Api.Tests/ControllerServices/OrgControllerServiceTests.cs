using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class OrgControllerServiceTests : BaseTests<OrgControllerService>
{

    private Mock<IOrganizationRepository>? _repositoryMock;
    private Mock<IRequestMapper>? _requestMapperMock;
    private Mock<IResponseMapper>? _responseMapperMock;

    [Theory]
    [InlineData("org")]
    public async Task GetOrgByCodeAsync_Successful_Status(string orgCode)
    {
        // Arrange
        var sut = Setup();

        _repositoryMock!
            .Setup(x => x.GetOrgByCodeAsync(orgCode))
            .ReturnsAsync(() => Fixture.Build<Organization>()
            .Create());

        // Act
        var (status, _) = await sut.GetOrgByCodeAsync(orgCode);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData("org")]
    public async Task GetOrgByCodeAsync_NonExistent_InformationMissing_Status(string orgCode)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetOrgByCodeAsync(orgCode))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetOrgByCodeAsync(orgCode);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Theory]
    [InlineData("org")]
    public async Task GetOrgByCodeAsync_UnknownErrorStatus(string orgCode)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetOrgByCodeAsync(orgCode))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.GetOrgByCodeAsync(orgCode);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task CreateOrgAsync_NullOrg_MissingInformationStatus()
    {
        //Arrange
        var sut = Setup();

        //Act
        var (status, _) = await sut.CreateOrgAsync(null);

        //Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task CreateOrgAsync_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<OrganizationRequestModel>();

        var org =
            Fixture
            .Build<Organization>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(org);

        _repositoryMock!
            .Setup(x => x.GetOrgByCodeAsync(It.IsAny<string>()))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.CreateOrgAsync(model);

        //Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task CreateOrgAsync_SuccessfulStatus()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<OrganizationRequestModel>();

        var org =
            Fixture
            .Build<Organization>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(org);

        _repositoryMock!
            .Setup(x => x.GetOrgByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(org);

        // Act
        var (status, _) = await sut.CreateOrgAsync(model);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);

    }

    [Fact]
    public async Task CreateOrgAsync_ReturnsMappedObject()
    {
        // Arrange
        var sut = Setup();

        var model =
            Fixture
            .Create<OrganizationRequestModel>();

        var expected =
            Fixture
            .Create<OrganizationResponse>();

        var org =
            Fixture
            .Build<Organization>()
            .Create();

        _requestMapperMock!
            .Setup(x => x.Map(model))
            .Returns(org);

        _responseMapperMock!
            .Setup(x => x.Map(org))
            .Returns(expected);

        _repositoryMock!
            .Setup(x => x.GetOrgByCodeAsync(It.IsAny<string>()))
            .ReturnsAsync(org);

        // Act
        var (_, actual) = await sut.CreateOrgAsync(model);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetOrgListAsync_Successful_Status()
    {
        // Arrange
        var sut = Setup();

        _repositoryMock!
            .Setup(x => x.GetOrgListAsync())
            .ReturnsAsync(() => Fixture.Build<List<Organization>>()
            .Create());

        // Act
        var (status, _) = await sut.GetOrgListAsync();

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task GetOrgListAsync_NonExistent_InformationMissing_Status()
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetOrgListAsync())
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetOrgListAsync();

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task GetOrgListAsync_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetOrgListAsync())
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.GetOrgListAsync();

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    protected override OrgControllerService Setup()
    {
        _repositoryMock = new();
        _requestMapperMock = new();
        _responseMapperMock = new();

        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(
            Mock.Of<ILogger<OrgControllerService>>(),
            _requestMapperMock.Object,
            _responseMapperMock.Object,
            _repositoryMock.Object);
    }

}
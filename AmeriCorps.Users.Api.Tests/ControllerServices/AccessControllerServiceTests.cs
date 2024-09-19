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
using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class RolesControllerServiceTests : BaseTests<RolesControllerService>
{

    private Mock<IRoleRepository>? _repositoryMock;
    private Mock<IRequestMapper>? _requestMapperMock;
    private Mock<IResponseMapper>? _responseMapperMock;
    private Mock<IValidator>? _validatorMock;

    [Theory]
    [InlineData("type")]
    public async Task GetRoleListByTypeAsync_Successful_Status(string role)
    {
        // Arrange
        var sut = Setup();

        _repositoryMock!
            .Setup(x => x.GetRoleListByTypeAsync(role))
            .ReturnsAsync(() => Fixture.Build<List<Role>>()
            .Create());

        // Act
        var (status, _) = await sut.GetRoleListByTypeAsync(role);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData("type")]
    public async Task GetRoleListByTypeAsync_NonExistent_InformationMissing_Status(string role)
    {
        // Arrange
        var sut = Setup();
        _repositoryMock!
            .Setup(x => x.GetRoleListByTypeAsync(role))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.GetRoleListByTypeAsync(role);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    protected override RolesControllerService Setup()
    {
        _repositoryMock = new();
        _requestMapperMock = new();
        _responseMapperMock = new();
        _validatorMock = new();

        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(
            Mock.Of<ILogger<RolesControllerService>>(),
            _requestMapperMock.Object,
            _responseMapperMock.Object,
            _validatorMock.Object,
            _repositoryMock.Object);
    }
}
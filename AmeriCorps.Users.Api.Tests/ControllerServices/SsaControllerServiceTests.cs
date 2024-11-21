using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class SsaControllerServiceTests : BaseTests<SsaControllerService>
{

    private Mock<IRequestMapper>? _requestMapperMock;
    private Mock<IResponseMapper>? _responseMapperMock;
    private Mock<IUserRepository>? _userRepositoryMock;
    private Mock<ISocialSecurityVerificationRepository>? _ssvRepositoryMock;
    private Mock<IEncryptionService>? _encryptionService;
    private Mock<IUserHelperService>? _userHelperService;


    [Fact]
    public async Task UpdateUserSSAInfoAsync_Successful_Status()
    {
        // Arrange
        var sut = Setup();

        var toUpdate =
            Fixture
            .Build<SocialSecurityVerificationRequestModel>()
            .Create();

        var update =
            Fixture
            .Build<SocialSecurityVerification>()
            .Create();

        _userRepositoryMock!
            .Setup(x => x.FindSocialSecurityVerificationByUserId(toUpdate.UserId))
            .ReturnsAsync(() => Fixture.Build<SocialSecurityVerification>()
            .Create());

        _ssvRepositoryMock!
            .Setup(x => x.SaveAsync(update))
            .ReturnsAsync(update);

        // Act
        var (status, _) = await sut.UpdateUserSSAInfoAsync(toUpdate);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS8604:Possible null reference argument for parameter", Justification = "Not production code. This is sumulating a null point to test exception")]
    public async Task UpdateUserSSAInfoAsync_NonExistent_InformationMissing_Status()
    {
        // Arrange
        var sut = Setup();

        var toUpdate =
            Fixture
            .Build<SocialSecurityVerificationRequestModel>()
            .Create();

        toUpdate.UserId = 0;

        _userRepositoryMock!
            .Setup(x => x.FindSocialSecurityVerificationByUserId(toUpdate.UserId))
            .ReturnsAsync(() => null);

        toUpdate = null;

        // Act
        var (status, _) = await sut.UpdateUserSSAInfoAsync(toUpdate);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Fact]
    public async Task UpdateUserSSAInfoAsync_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();

        var toUpdate =
            Fixture
            .Build<SocialSecurityVerificationRequestModel>()
            .Create();

        var update =
            Fixture
            .Build<SocialSecurityVerification>()
            .Create();

        _userRepositoryMock!
            .Setup(x => x.FindSocialSecurityVerificationByUserId(toUpdate.UserId))
            .ThrowsAsync(new Exception());

        _ssvRepositoryMock!
            .Setup(x => x.SaveAsync(update))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.UpdateUserSSAInfoAsync(toUpdate);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    public async Task SubmitInfoForVerificationAsync_Successful_Status(int userId)
    {
        // Arrange
        var sut = Setup();

        var update =
            Fixture
            .Build<SocialSecurityVerification>()
            .Create();

        _userRepositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => Fixture.Build<User>().Without(u => u.SocialSecurityVerification)
            .Create());

        _userRepositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => Fixture.Build<User>()
            .Create());

        _ssvRepositoryMock!
            .Setup(x => x.SaveAsync(update))
            .ReturnsAsync(update);

        // Act
        var (status, _) = await sut.SubmitInfoForVerificationAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(0)]
    public async Task SubmitInfoForVerificationAsync_NonExistent_InformationMissing_Status(int userId)
    {
        // Arrange
        var sut = Setup();

        _userRepositoryMock!
            .Setup(x => x.GetAsync(0))
            .ReturnsAsync(() => Fixture.Build<User>()
            .Create());

        _userRepositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ReturnsAsync(() => Fixture.Build<User>().Without(u => u.EncryptedSocialSecurityNumber)
            .Create());

        // Act
        var (status, _) = await sut.SubmitInfoForVerificationAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    public async Task SubmitInfoForVerificationAsync_UnknownErrorStatus(int userId)
    {
        // Arrange
        var sut = Setup();

        var update =
            Fixture
            .Build<SocialSecurityVerification>()
            .Create();

        _userRepositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ThrowsAsync(new Exception());

        _ssvRepositoryMock!
            .Setup(x => x.SaveAsync(update))
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.SubmitInfoForVerificationAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Fact]
    public async Task FetchPendingUsersForSSAVerificationAsync_Successful_Status()
    {
        // Arrange
        var sut = Setup();


        _userRepositoryMock!
            .Setup(x => x.FetchPendingUsersForSSAVerificationAsync())
            .ReturnsAsync(() => Fixture.Build<List<User>>()
            .Create());

        // Act
        var (status, _) = await sut.FetchPendingUsersForSSAVerificationAsync();

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task FetchPendingUsersForSSAVerificationAsync_UnknownErrorStatus()
    {
        // Arrange
        var sut = Setup();

        _userRepositoryMock!
            .Setup(x => x.FetchPendingUsersForSSAVerificationAsync())
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.FetchPendingUsersForSSAVerificationAsync();

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(0)]
    public async Task NotifyFailedUserVerificationsAsync_Successful_Status(int userId)
    {
        // Arrange
        var sut = Setup();

        if(userId > 0)
        {
            _userRepositoryMock!
                .Setup(x => x.GetAsync(userId))
                .ReturnsAsync(() => Fixture.Build<User>()
                .Create());
        }
        else
        {
            _userRepositoryMock!
                .Setup(x => x.FetchFailedSSAChecksAsync())
                .ReturnsAsync(() => Fixture.Build<List<User>>()
                .Create());
        }

        // Act
        var (status, _) = await sut.NotifyFailedUserVerificationsAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.Successful, status);
    }

    [Fact]
    public async Task NotifyFailedUserVerificationsAsync_NonExistent_InformationMissing_Status()
    {
        // Arrange
        var sut = Setup();

        _userRepositoryMock!
            .Setup(x => x.FetchFailedSSAChecksAsync())
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.NotifyFailedUserVerificationsAsync(0);

        // Assert
        Assert.Equal(ResponseStatus.MissingInformation, status);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(0)]
    public async Task NotifyFailedUserVerificationsAsync_UnknownErrorStatus(int userId)
    {
        // Arrange
        var sut = Setup();

        _userRepositoryMock!
            .Setup(x => x.GetAsync(userId))
            .ThrowsAsync(new Exception());

        _userRepositoryMock!
            .Setup(x => x.FetchFailedSSAChecksAsync())
            .ThrowsAsync(new Exception());

        // Act
        var (status, _) = await sut.NotifyFailedUserVerificationsAsync(userId);

        // Assert
        Assert.Equal(ResponseStatus.UnknownError, status);
    }

    protected override SsaControllerService Setup()
    {
        _requestMapperMock = new();
        _responseMapperMock = new();
        _userRepositoryMock = new();
        _ssvRepositoryMock = new();
        _encryptionService = new();
        _userHelperService = new();

        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(
            Mock.Of<ILogger<SsaControllerService>>(),
            _requestMapperMock.Object,
            _responseMapperMock.Object,
            _userRepositoryMock.Object,
            _encryptionService.Object,
            _ssvRepositoryMock.Object,
            _userHelperService.Object);
    }
}
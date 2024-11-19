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
    public async Task UpdateUserSSAInfoAsync_NonExistent_InformationMissing_Status()
    {
        // Arrange
        var sut = Setup();

        var toUpdate =
            Fixture
            .Build<SocialSecurityVerificationRequestModel>()
            .Create();

        _userRepositoryMock!
            .Setup(x => x.FindSocialSecurityVerificationByUserId(toUpdate.UserId))
            .ReturnsAsync(() => null);

        // Act
        var (status, _) = await sut.UpdateUserSSAInfoAsync(null);

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
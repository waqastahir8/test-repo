using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Logging;
using AmeriCorps.Users.Http;
using AmeriCorps.Users.Data.Core.Model;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class UserHelperServiceTests : BaseTests<UserHelperService>
{
    private Mock<IUserRepository>? _repositoryMock;
    private Mock<INotificationApiClient>? _apiServiceMock;
    private Mock<IEmailTemplatesService>? _templatesMock;


    [Fact]
    public async Task SendUserInviteAsync_Successful_Response()
    {
        // Arrange
        var sut = Setup();

        var toInvite =
            Fixture
            .Build<User>()
            .Create();

        var userEmail =
            Fixture
            .Build<CommunicationMethod>()
            .Create();

        toInvite.FirstName = "test";
        toInvite.LastName = "name";
        toInvite.CommunicationMethods.Add(userEmail);

        var email =
            Fixture
            .Build<EmailModel>()
            .Create();


        var successfulResponse =
            Fixture
                .Build<ServiceResponse<UserResponse>>()
                .With(x => x.Successful, true)
                .Create();


        _apiServiceMock!
            .Setup(x => x.SendInviteEmailAsync(email))
            .ReturnsAsync(successfulResponse);


        // Act
         var actual = await sut.SendUserInviteAsync(toInvite);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task SendUserInviteAsync_ThrowsException_UnsuccessfulResponse()
    {
        // Arrange
        var sut = Setup();

        var toInvite =
            Fixture
            .Build<User>()
            .Create();
            
        toInvite.FirstName = "";
        toInvite.LastName = "";

        // Act
         var actual = await sut.SendUserInviteAsync(toInvite);

        // Assert
        Assert.False(actual);
    }


    [Fact]
    public async Task ResendUserInviteAsync_Successful_Response()
    {
        // Arrange
        var sut = Setup();

        var toInvite =
            Fixture
            .Build<User>()
            .Create();      

        var userEmail =
            Fixture
            .Build<CommunicationMethod>()
            .Create();

        DateTime dateInvited = new DateTime(2024, 6, 7, 12, 30, 0, DateTimeKind.Utc);

        toInvite.UserAccountStatus = UserAccountStatus.INVITED;
        toInvite.InviteDate = dateInvited;
        toInvite.CommunicationMethods.Add(userEmail);

        var email =
            Fixture
            .Build<EmailModel>()
            .Create();


        var successfulResponse =
            Fixture
                .Build<ServiceResponse<UserResponse>>()
                .With(x => x.Successful, true)
                .Create();


        _apiServiceMock!
            .Setup(x => x.SendInviteEmailAsync(email))
            .ReturnsAsync(successfulResponse);


        // Act
         var actual = await sut.ResendUserInviteAsync(toInvite);

        // Assert
        Assert.True(actual);
    }

    [Fact]
    public async Task ResendUserInviteAsync_ThrowsException_UnsuccessfulResponse()
    {
        // Arrange
        var sut = Setup();

        var toInvite =
            Fixture
            .Build<User>()
            .Create();      

        toInvite.UserAccountStatus = UserAccountStatus.ACTIVE;

        // Act
         var actual = await sut.ResendUserInviteAsync(toInvite);

        // Assert
        Assert.False(actual);
    }


    [Fact]
    public async Task ResendAllUserInvitesAsync_Successful_Response()
    {
        // Arrange
        var sut = Setup();

        DateTime dateInvited = new DateTime(2024, 6, 7, 12, 30, 0, DateTimeKind.Utc);

        var userId =
            Fixture
            .Create<int>();

        var userEmail =
            Fixture
            .Build<List<CommunicationMethod>>()
            .Create();

        var user =
            Fixture
            .Build<User>()
            .With(o => o.InviteDate, dateInvited)
            .With(o => o.UserAccountStatus, UserAccountStatus.INVITED)
            .With(o => o.CommunicationMethods, userEmail)
            .Create();

        _repositoryMock!
            .Setup(x => x.SaveAsync(user))
            .ReturnsAsync(user);

        var email =
            Fixture
            .Build<EmailModel>()
            .Create();

        var successfulResponse =
            Fixture
                .Build<ServiceResponse<UserResponse>>()
                .With(x => x.Successful, true)
                .Create();


        _apiServiceMock!
            .Setup(x => x.SendInviteEmailAsync(email))
            .ReturnsAsync(successfulResponse);


        // Act
         var actual = await sut.ResendAllUserInvitesAsync();

        // Assert
        Assert.True(actual);
    }

    // [Fact]
    // public async Task ResendAllUserInvitesAsync_ThrowsException_UnsuccessfulResponse()
    // {
    //     // Arrange
    //     var sut = Setup();

    //     DateTime dateInvited = new DateTime(2024, 6, 7, 12, 30, 0, DateTimeKind.Utc);

    //     var user =
    //         Fixture
    //         .Build<User>()
    //         // .With(o => o.InviteDate, dateInvited)
    //         .With(o => o.UserAccountStatus, UserAccountStatus.INVITED)
    //         .Without(u => u.CommunicationMethods)
    //         .Create();
            
    //     _repositoryMock!
    //         .Setup(x => x.SaveAsync(user))
    //         .ReturnsAsync(user);

    //     // Act
    //      var actual = await sut.ResendAllUserInvitesAsync();

    //     // Assert
    //     Assert.False(actual);
    // }


    protected override UserHelperService Setup()
    {
        _repositoryMock = new();
        _apiServiceMock = new();
        _templatesMock = new();

        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(
            Mock.Of<ILogger<UserHelperService>>(),
            _repositoryMock.Object,
            _apiServiceMock.Object,
            _templatesMock.Object);
    }
}
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Http;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class ApiServiceTests : BaseTests<ApiService>
{

    private Mock<INotificationApiClient>? _apiClientMock;


    [Fact]
    public async Task SendInviteEmailAsync_ApiCallThrowsException_UnsuccessfulResponse()
    {
        // Arrange
        var sut = Setup();
        var email = Fixture.Create<EmailModel>();

        _apiClientMock!
            .Setup(x => x.SendInviteEmailAsync(email))
            .ThrowsAsync(new Exception());

        // Act
        var actual = await sut.SendInviteEmailAsync(email);

        // Assert
        Assert.False(actual.Item1);
        Assert.Null(actual.Item2);
    }

    [Fact]
    public async Task SendInviteEmailAsync_SuccessfulApiCall_SuccessfulResponse()
    {
        var sut = Setup();

        var email = Fixture.Create<EmailModel>();

        
        var successfulResponse =
            Fixture
                .Build<ServiceResponse<UserResponse>>()
                .With(x => x.Successful, true)
                .Create();

        _apiClientMock!
            .Setup(x => x.SendInviteEmailAsync(email))
            .ReturnsAsync(successfulResponse);

        // Act
        var actual = await sut.SendInviteEmailAsync(email);

        // Assert
        Assert.True(actual.Item1);
    }


    protected override ApiService Setup()
    {
        _apiClientMock = new();
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(_apiClientMock.Object);
    }
}
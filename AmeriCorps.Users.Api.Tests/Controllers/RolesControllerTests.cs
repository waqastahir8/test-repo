using AmeriCorps.Users.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class RolesControllerTests : BaseTests<RolesController>
{
    private Mock<IRolesControllerService>? _serviceMock;


    [Fact]
    public async Task GetRoleListByTypeAsync_UnknownError_500StatusCode()
    {
        //Arrange
        var sut = Setup();
        var org = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetRoleListByTypeAsync(org))
            .ReturnsAsync((ResponseStatus.UnknownError, null));
        //Act
        var actual = await sut.GetRoleListByTypeAsync(org);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }


    [Fact]
    public async Task GetRoleListByTypeAsync_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var org = Fixture.Create<string>();
        var projResponse = Fixture.Create<List<RoleResponse>>();

        _serviceMock!
            .Setup(x => x.GetRoleListByTypeAsync(org))
            .ReturnsAsync((ResponseStatus.Successful, projResponse));
        //Act
        var actual = await sut.GetRoleListByTypeAsync(org);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }


    [Fact]
    public async Task GetRoleListByTypeAsync_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var org = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetRoleListByTypeAsync(org))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.GetRoleListByTypeAsync(org);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }


    protected override RolesController Setup()
    {
        _serviceMock = new();
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(_serviceMock.Object);
    }

}
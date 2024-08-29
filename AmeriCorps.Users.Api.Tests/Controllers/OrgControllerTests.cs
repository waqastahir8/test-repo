using AmeriCorps.Users.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class OrgControllerTests : BaseTests<OrgController>
{
    private Mock<IOrgControllerService>? _serviceMock;


    [Fact]
    public async Task GetOrgByCode_SuccessProcessing_200StatusCode()
    {
        //Arrange
        var sut = Setup();
        var orgCode = Fixture.Create<string>();
        var orgResponse = Fixture.Create<OrganizationResponse>();

        _serviceMock!
            .Setup(x => x.GetOrgByCode(orgCode))
            .ReturnsAsync((ResponseStatus.Successful, orgResponse));
        //Act
        var actual = await sut.GetOrgByCode(orgCode);

        //Assert
        var response = actual as OkObjectResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
    }
    protected override OrgController Setup()
    {
        _serviceMock = new();
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new(_serviceMock.Object);
    }
}
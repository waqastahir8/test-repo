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

        [Fact]
    public async Task GetOrgByCode_NonExistent_UnprocessableEntity_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var orgCode = Fixture.Create<string>();

        _serviceMock!
            .Setup(x => x.GetOrgByCode(orgCode))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));
        //Act
        var actual = await sut.GetOrgByCode(orgCode);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task CreateOrg_InformationMissing_422StatusCode()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture.Create<OrganizationResponse>();

        _serviceMock!
            .Setup(x => x.CreateOrg(model))
            .ReturnsAsync((ResponseStatus.MissingInformation, null));

        //Act
        var actual = await sut.CreateOrg(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.UnprocessableEntity, response.StatusCode);
    }

    [Fact]
    public async Task CreateOrg_UnknownError_500StatusCode()
    {

        //Arrange
        var sut = Setup();
        var model = Fixture.Create<OrganizationRequestModel>();

        _serviceMock!
            .Setup(x => x.CreateOrg(model))
            .ReturnsAsync((ResponseStatus.UnknownError, null));

        //Act
        var actual = await sut.CreateOrg(model);

        //Assert
        var response = actual as StatusCodeResult;
        Assert.NotNull(response);
        Assert.Equal((int)HttpStatusCode.InternalServerError, response.StatusCode);
    }

    [Fact]
    public async Task CreateOrg_SuccessProcessing_200StatusCode()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<OrganizationRequestModel>();

        _serviceMock!
            .Setup(x => x.CreateOrg(model))
            .ReturnsAsync((ResponseStatus.Successful, Fixture.Create<OrganizationResponse>()));

        // Act
        var actual = await sut.CreateOrg(model);

        // Assert
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
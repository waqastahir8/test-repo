using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class ReferenceRequestMapperTests : RequestMapperSetup
{

    [Fact]
    public void Map_CorrectlyMapsProperties()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<ReferenceRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.TypeId, result.TypeId);
        Assert.Equal(model.Relationship, result.Relationship);
        Assert.Equal(model.RelationshipLength, result.RelationshipLength);
        Assert.Equal(model.ContactName, result.ContactName);
        Assert.Equal(model.Email, result.Email);
        Assert.Equal(model.Phone, result.Phone);
        Assert.Equal(model.Address, result.Address);
        Assert.Equal(model.Company, result.Company);
        Assert.Equal(model.Position, result.Position);
        Assert.Equal(model.Notes, result.Notes);
        Assert.Equal(model.CanContact, result.CanContact);
        Assert.Equal(model.Contacted, result.Contacted);
        Assert.Equal(model.DateContacted, result.DateContacted);
    }
}
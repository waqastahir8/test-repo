using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class ReferenceResponseMapperTests : ResponseMapperSetup
{

    [Fact]
    public void Map_CorrectlyMapsProperties()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<Reference>();

        IResponseMapper mapper = new ResponseMapper();

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

    [Fact]
    public void Map_CorrectlyMapsReferenceList()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<List<Reference>>();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.All(model.Zip(result, (source, mapped) => (source, mapped)),
            pair =>
            {
                Assert.Equal(pair.source.TypeId, pair.mapped.TypeId);
                Assert.Equal(pair.source.Relationship, pair.mapped.Relationship);
                Assert.Equal(pair.source.RelationshipLength, pair.mapped.RelationshipLength);
                Assert.Equal(pair.source.ContactName, pair.mapped.ContactName);
                Assert.Equal(pair.source.Email, pair.mapped.Email);
                Assert.Equal(pair.source.Phone, pair.mapped.Phone);
                Assert.Equal(pair.source.Address, pair.mapped.Address);
                Assert.Equal(pair.source.Company, pair.mapped.Company);
                Assert.Equal(pair.source.Position, pair.mapped.Position);
                Assert.Equal(pair.source.Notes, pair.mapped.Notes);
                Assert.Equal(pair.source.CanContact, pair.mapped.CanContact);
                Assert.Equal(pair.source.Contacted, pair.mapped.Contacted);
                Assert.Equal(pair.source.DateContacted, pair.mapped.DateContacted);
            });
    }
}
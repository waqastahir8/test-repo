using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class CollectionResponseMapperTests : ResponseMapperSetup
{

    [Fact]
    public void Map_CorrectlyMapsProperties()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<Collection>();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.Id, result.Id);
        Assert.Equal(model.Type, result.Type.ToUpper());
        Assert.Equal(model.UserId, result.UserId);
        Assert.Equal(model.ListingId, result.ListingId);
    }

    [Fact]
    public void Map_CorrectlyMapsCollectionList()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<List<Collection>>();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.All(model.Zip(result.Listings, (source, mapped) => (source, mapped)),
            pair =>
            {
                Assert.Equal(pair.source.ListingId, pair.mapped);
            });
    }

    [Fact]
    public void Map_CorrectlyMapsEmptyCollectionList()
    {
        // Arrange
        var sut = Setup();
        var model = new List<Collection>();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.Count, result.Listings.Count);
    }

    [Fact]
    public void Map_CorrectlyMapsNullCollectionList()
    {
        // Arrange
        var sut = Setup();
        List<Collection> model = null;

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Empty(result.Listings);
    }
}
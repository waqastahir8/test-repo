using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class CollectionRequestMapperTests : RequestMapperSetup
{

    [Fact]
    public void Map_CorrectlyMapsProperties()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<CollectionRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.UserId, result.UserId);
        Assert.Equal(model.ListingId, result.ListingId);
    }

    [Fact]
    public void Map_CorrectlyMapsCollectionList()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<CollectionListRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.All(model.Listings.Zip(result, (source, mapped) => (source, mapped)),
            pair =>
            {
                Assert.Equal(model.Type.ToUpper(), pair.mapped.Type);
                Assert.Equal(model.UserId, pair.mapped.UserId);
                Assert.Equal(pair.source, pair.mapped.ListingId);
            });
    }
}
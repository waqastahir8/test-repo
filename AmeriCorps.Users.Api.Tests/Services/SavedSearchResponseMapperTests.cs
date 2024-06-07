using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class SavedSearchResponseMapperTests : ResponseMapperSetup
{

    [Fact]
    public void Map_CorrectlyMapsProperties()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<SavedSearch>();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.Id, result.Id);
        Assert.Equal(model.UserId, result.UserId);
        Assert.Equal(model.Name, result.Name);
        Assert.Equal(model.Filters, result.Filters);
        Assert.Equal(model.NotificationsOn, result.NotificationsOn);
    }

    [Fact]
    public void Map_CorrectlyMapsSearchList()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<List<SavedSearch>>();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.All(model.Zip(result, (source, mapped) => (source, mapped)),
            pair =>
            {
                Assert.Equal(pair.source.Id, pair.mapped.Id);
                Assert.Equal(pair.source.UserId, pair.mapped.UserId);
                Assert.Equal(pair.source.Name, pair.mapped.Name);
                Assert.Equal(pair.source.Filters, pair.mapped.Filters);
                Assert.Equal(pair.source.NotificationsOn, pair.mapped.NotificationsOn);
            });
    }
}
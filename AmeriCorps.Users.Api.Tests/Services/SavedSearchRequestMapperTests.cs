using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class SavedSearchRequestMapperTests : RequestMapperSetup
{

    [Fact]
    public void Map_CorrectlyMapsProperties()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<SavedSearchRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.Id, result.Id);
        Assert.Equal(model.UserId, result.UserId);
        Assert.Equal(model.Name, result.Name);
        Assert.Equal(model.Filters, result.Filters);
        Assert.Equal(model.NotificationsOn, result.NotificationsOn);
    }
}
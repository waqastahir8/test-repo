using AmeriCorps.Users.Api.Helpers.Collection;

namespace AmeriCorps.Users.Api.Tests;

public sealed partial class CollectionHelpersTests
{
    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("cart", true)]
    [InlineData("CART", true)]
    [InlineData("Cart", true)]
    [InlineData("favourite", true)]
    [InlineData("FAVOURITE", true)]
    [InlineData("Favourite", true)]
    [InlineData("unknown", false)]
    [InlineData("TYPE", false)]
    public void ContainsTypeTests(string? input, bool expected)
    {
        // Act
        bool result = CollectionHelpers.ContainsType(input);

        // Assert
        Assert.Equal(expected, result);
    }
}
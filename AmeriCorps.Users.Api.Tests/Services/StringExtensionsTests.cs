namespace AmeriCorps.Users.Api.Tests;

public sealed class StringExtensionsTests
{
    [Theory]
    [InlineData("   key", "key")]
    [InlineData("key      ", "key")]
    [InlineData("        key      ", "key")]
    public void Sanitize_TrailingSpaces_SpacesRemoved(string value, string expected)
    {
        // Act
        var actual = value.Sanitize();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData("KEY", "key")]
    [InlineData("KeY", "key")]
    [InlineData("Key", "key")]
    [InlineData("kEY", "key")]
    public void Sanitize_UpperCaseLetters_ConvertsToLowerCase(string value, string expected)
    {
        // Act
        var actual = value.Sanitize();

        // Assert
        Assert.Equal(expected, actual);
    }
}

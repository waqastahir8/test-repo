using System.Linq.Expressions;
namespace AmeriCorps.Users.Api.Tests;

public sealed class ValidatorTests : BaseTests<Validator>
{

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("\t\t\r\n")]
    [InlineData("      ")]
    public void Validate_MissingFirstName_Fails(string? value) =>
        AssertUserValidationFailsOnMissingProperty(u => u.FirstName, value);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("\t\t\r\n")]
    [InlineData("      ")]
    public void Validate_MissingLastName_Fails(string? value) =>
        AssertUserValidationFailsOnMissingProperty(u => u.LastName, value);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("\t\t\r\n")]
    [InlineData("      ")]
    public void Validate_MissingUserName_Fails(string? value) =>
        AssertUserValidationFailsOnMissingProperty(u => u.UserName, value);

    [Fact]
    public void Validate_Over18_DateOfBirth_Succeeds()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture
            .Build<UserRequestModel>()
            .With(x => x.DateOfBirth,
                    DateOnly.FromDateTime(
                    DateTime.Today.AddYears(-18)))
            .Create();

        //Act
        var valid = sut.Validate(model);

        //Assert
        Assert.True(valid);
    }

    [Fact]
    public void Validate_Under18_DateOfBirth_Fails()
    {
        //Arrange
        var sut = Setup();
        var model = Fixture
            .Build<UserRequestModel>()
            .With(x => x.DateOfBirth,
                    DateOnly.FromDateTime(
                    DateTime.Today.AddYears(-17).AddDays(-364)))
            .Create();

        //Act
        var valid = sut.Validate(model);

        //Assert
        Assert.False(valid);
    }


    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("\t\t\r\n")]
    [InlineData("      ")]
    public void Validate_MissingSearchName_Fails(string? value) =>
        AssertSearchValidationFailsOnMissingProperty(s => s.Name, value);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("\t\t\r\n")]
    [InlineData("      ")]
    public void Validate_MissingSearchFilters_Fails(string? value) =>
        AssertSearchValidationFailsOnMissingProperty(s => s.Filters, value);
    private void AssertUserValidationFailsOnMissingProperty<T>(
        Expression<Func<UserRequestModel, T>> propertyPicker,
        T value)
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Build<UserRequestModel>()
            .With(propertyPicker, value)
            .Create();

        // Act
        var actual = sut.Validate(model);

        // Assert
        Assert.False(actual);
    }

    private void AssertSearchValidationFailsOnMissingProperty<T>(
        Expression<Func<SavedSearchRequestModel, T>> propertyPicker,
        T value)
    {
        // Arrange
        var sut = Setup();
        var model =
            Fixture
            .Build<SavedSearchRequestModel>()
            .With(propertyPicker, value)
            .Create();

        // Act
        var actual = sut.Validate(model);

        // Assert
        Assert.False(actual);
    }

    protected override Validator Setup()
    {
        Fixture = new();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new();
    }
}
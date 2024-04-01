using Xunit;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;

namespace AmeriCorps.Users.Api.Tests;

public sealed class RequestMapperTests : BaseTests<RequestMapper>
{
    [Fact]
    public void Map_CorrectlyMapsProperties()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<UserRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.FirstName, result.FirstName);
        Assert.Equal(model.LastName, result.LastName);
        Assert.Equal(model.MiddleName, result.MiddleName);
        Assert.Equal(model.UserName, result.UserName);
        Assert.Equal(model.ExternalAccountId, result.ExternalAccountId);
        Assert.Equal(model.PreferredName, result.PreferredName);
        Assert.Equal(model.UserName, result.UserName);
        Assert.Equal(model.DateOfBirth, result.DateOfBirth);

        //Assert attributes
        Assert.Equal(model.Attributes.Count, result.Attributes.Count);

        Assert.All(result.Attributes.Zip(model.Attributes, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.Type, pair.mapped.Type);
                Assert.Equal(pair.source.Value, pair.mapped.Value);
            });

        //Assert languages
        Assert.Equal(model.Languages.Count, result.Languages.Count);

        Assert.All(result.Languages.Zip(model.Languages, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.PickListId, pair.mapped.PickListId);
                Assert.Equal(pair.source.IsPrimary, pair.mapped.IsPrimary);
                Assert.Equal(pair.source.SpeakingAbility, pair.mapped.SpeakingAbility);
                Assert.Equal(pair.source.WritingAbility, pair.mapped.WritingAbility);
            });

        //Assert addresses
        Assert.Equal(model.Addresses.Count, result.Addresses.Count);
        Assert.All(result.Addresses.Zip(model.Addresses, (mapped, source) => (mapped, source)),
        pair =>
        {
            Assert.Equal(pair.source.IsForeign, pair.mapped.IsForeign);
            Assert.Equal(pair.source.Type, pair.mapped.Type);
            Assert.Equal(pair.source.Street1, pair.mapped.Street1);
            Assert.Equal(pair.source.Street2, pair.mapped.Street2);
            Assert.Equal(pair.source.City, pair.mapped.City);
            Assert.Equal(pair.source.State, pair.mapped.State);
            Assert.Equal(pair.source.Country, pair.mapped.Country);
            Assert.Equal(pair.source.ZipCode, pair.mapped.ZipCode);
            Assert.Equal(pair.source.MovingWithinSixMonths, pair.mapped.MovingWithinSixMonths);
        });

        //Assert education
        Assert.Equal(model.Education.Count, result.Education.Count);
        Assert.All(result.Education.Zip(model.Education, (mapped, source) => (mapped, source)),
        pair =>
        {
            Assert.Equal(pair.source.Level, pair.mapped.Level);
            Assert.Equal(pair.source.MajorAreaOfStudy, pair.mapped.MajorAreaOfStudy);
            Assert.Equal(pair.source.Institution, pair.mapped.Institution);
            Assert.Equal(pair.source.City, pair.mapped.City);
            Assert.Equal(pair.source.State, pair.mapped.State);
            Assert.Equal(pair.source.DateAttendedFrom, pair.mapped.DateAttendedFrom);
            Assert.Equal(pair.source.DateAttendedTo, pair.mapped.DateAttendedTo);
            Assert.Equal(pair.source.DegreeTypePursued, pair.mapped.DegreeTypePursued);
            Assert.Equal(pair.source.DegreeCompleted, pair.mapped.DegreeCompleted);
        });

        //Assert Skills
        Assert.Equal(model.Skills.Count, result.Skills.Count);

        Assert.All(result.Skills.Zip(model.Skills, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.PickListId, pair.mapped.PickListId);
            });

        //Assert Military Service
        Assert.Equal(model.MilitaryService.Count, result.MilitaryService.Count);

        Assert.All(result.MilitaryService.Zip(model.MilitaryService, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.PickListId, pair.mapped.PickListId);
            });

        //Assert Saved Searches
        Assert.Equal(model.SavedSearches.Count, result.SavedSearches.Count);

        Assert.All(result.SavedSearches.Zip(model.SavedSearches, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.UserId, pair.mapped.UserId);
                Assert.Equal(pair.source.Name, pair.mapped.Name);
                Assert.Equal(pair.source.Filters, pair.mapped.Filters);
                Assert.Equal(pair.source.NotificationsOn, pair.mapped.NotificationsOn);
            });

        //Assert Relatives
        Assert.Equal(model.Relatives.Count, result.Relatives.Count);

        Assert.All(result.Relatives.Zip(model.Relatives, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.Relationship, pair.mapped.Relationship);
                Assert.Equal(pair.source.HighestEducationLevel, pair.mapped.HighestEducationLevel);
                Assert.Equal(pair.source.AnnualIncome, pair.mapped.AnnualIncome);
            });

        //Assert Communication Methods
        Assert.Equal(model.CommunicationMethods.Count, result.CommunicationMethods.Count);

        Assert.All(result.CommunicationMethods.Zip(model.CommunicationMethods, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.Type, pair.mapped.Type);
                Assert.Equal(pair.source.Value, pair.mapped.Value);
                Assert.Equal(pair.source.IsPreferred, pair.mapped.IsPreferred);
            });
    }

    protected override RequestMapper Setup()
    {
        Fixture = new Fixture();
        Fixture.Customize<DateOnly>(x => x.FromFactory<DateTime>(DateOnly.FromDateTime));
        return new();
    }
}
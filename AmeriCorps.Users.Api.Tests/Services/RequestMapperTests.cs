using Xunit;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Data.Core;
using System.Linq;

namespace AmeriCorps.Users.Api.Tests;

public sealed class RequestMapperTests : RequestMapperSetup
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
        Assert.Equal(model.Pronouns, result.Pronouns);
        Assert.Equal(model.OrgCode, result.OrgCode);

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

        //Assert References
        Assert.Equal(model.References.Count, result.References.Count);

        Assert.All(result.References.Zip(model.References, (mapped, source) => (mapped, source)),
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

        //Assert User Project Methods
        Assert.Equal(model.UserProjects.Count, result.UserProjects.Count);

        Assert.All(result.UserProjects.Zip(model.UserProjects, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.ProjectName, pair.mapped.ProjectName);
                Assert.Equal(pair.source.ProjectCode, pair.mapped.ProjectCode);
                Assert.Equal(pair.source.ProjectType, pair.mapped.ProjectType);
                Assert.Equal(pair.source.ProjectOrg, pair.mapped.ProjectOrg);
                Assert.Equal(pair.source.Active, pair.mapped.Active);
            });

        TestUserCollectionRequestMapper(result, model);
    }

    [Fact]
    public void Map_CorrectlyMapsOrganizationRequest()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<OrganizationRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert

        Assert.Equal(model.OrgName, result.OrgName);
        Assert.Equal(model.OrgCode, result.OrgCode);
        Assert.Equal(model.Description, result.Description);
    }

    [Fact]
    public void Map_CorrectlyMapsProjecRequest()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<ProjectRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert

        Assert.Equal(model.ProjectName, result.ProjectName);
        Assert.Equal(model.ProjectCode, result.ProjectCode);
        Assert.Equal(model.ProjectType, result.ProjectType);
        Assert.Equal(model.ProjectOrgCode, result.ProjectOrgCode);
        Assert.Equal(model.Description, result.Description);
    }

    [Fact]
    public void Map_CorrectlyMapsAccessRequest()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<AccessRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert

        Assert.Equal(model.AccessName, result.AccessName);
        Assert.Equal(model.AccessLevel, result.AccessLevel);
        Assert.Equal(model.AccessType, result.AccessType);
        Assert.Equal(model.Description, result.Description);
    }

    [Fact]
    public void Map_CorrectlyMapsRoleRequest()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<RoleRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert

        Assert.Equal(model.RoleName, result.RoleName);
        Assert.Equal(model.FunctionalName, result.FunctionalName);
        Assert.Equal(model.Description, result.Description);
    }

    [Fact]
    public void Map_CorrectlyMapsUserRoleRequest()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<Role>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.RoleName, result.RoleName);
        Assert.Equal(model.FunctionalName, result.FunctionalName);
    }

    [Fact]
    public void Map_CorrectlyMapsOperatingSiteRequest()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<OperatingSiteRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.ProgramYear, result.ProgramYear);
        Assert.Equal(model.Active, result.Active);
        Assert.Equal(model.OperatingSiteName, result.OperatingSiteName);
        Assert.Equal(model.ContactName, result.ContactName);
        Assert.Equal(model.EmailAddress, result.EmailAddress);
        Assert.Equal(model.PhoneNumber, result.PhoneNumber);
        Assert.Equal(model.StreetAddress, result.StreetAddress);
        Assert.Equal(model.StreetAddress2, result.StreetAddress2);
        Assert.Equal(model.City, result.City);
        Assert.Equal(model.State, result.State);
        Assert.Equal(model.ZipCode, result.ZipCode);
        Assert.Equal(model.Plus4, result.Plus4);
        Assert.Equal(model.InviteDate, result.InviteDate);
        Assert.Equal(model.InviteUserId, result.InviteUserId);
    }

    [Fact]
    public void Map_CorrectlyMapsAwardRequest()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Build<AwardResponse>()
            .Without(a => a.PerformanceStartDt)
            .Without(a => a.PerformanceEndDt)
            .Create();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.AwardCode, result.AwardCode);
        Assert.Equal(model.AwardName, result.AwardName);
        Assert.Equal(model.GspListingNumber, result.GspListingNumber);
        Assert.Equal(model.Fain, result.Fain);
        Assert.Equal(model.Uei, result.Uei);

        Assert.Equal(model.PerformanceStartDt, result.PerformanceStartDt);
        Assert.Equal(model.PerformanceEndDt, result.PerformanceEndDt);
    }

    [Fact]
    public void Map_CorrectlyMapsSubGranteeRequest()
    {
        // Arrange
        var sut = Setup();
        var model = Fixture.Create<SubGranteeRequestModel>();

        IRequestMapper mapper = new RequestMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        Assert.Equal(model.GranteeCode, result.GranteeCode);
        Assert.Equal(model.GranteeName, result.GranteeName);
        Assert.Equal(model.Uei, result.Uei);
        Assert.Equal(model.StreetAddress, result.StreetAddress);
        Assert.Equal(model.City, result.City);
        Assert.Equal(model.State, result.State);
        Assert.Equal(model.ZipCode, result.ZipCode);
    }

    private void TestUserCollectionRequestMapper(User result, UserRequestModel model)
    {
        Assert.Equal(model.Collection.Count, result.Collection.Count);
        Assert.All(result.Collection.Zip(model.Collection, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.Type.ToUpper(), pair.mapped.Type);
                Assert.Equal(pair.source.UserId, pair.mapped.UserId);
                Assert.Equal(pair.source.ListingId, pair.mapped.ListingId);
            });
    }
}
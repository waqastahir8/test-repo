using AmeriCorps.Users.Api.Services;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Tests;

public sealed class ResponseMapperTests : ResponseMapperSetup
{
    [Fact]
    public void Map_CorrectlyMapsProperties()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<User>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
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
        //Assert User Role Methods
        Assert.Equal(model.Roles.Count, result.UserRoles.Count);

        Assert.All(result.UserRoles.Zip(model.Roles, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.RoleName, pair.mapped.RoleName);
                Assert.Equal(pair.source.FunctionalName, pair.mapped.FunctionalName);
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

        TestUserCollectionResponseMapper(result, model);
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesOrganizationResponse()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<Organization>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.OrgName, result.OrgName);
        Assert.Equal(model.OrgCode, result.OrgCode);
        Assert.Equal(model.Description, result.Description);
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesProjectResponse()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<Project>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.ProjectName, result.ProjectName);
        Assert.Equal(model.ProjectOrgCode, result.ProjectOrgCode);
        Assert.Equal(model.ProjectCode, result.ProjectCode);

        Assert.Equal(model.ProjectId, result.ProjectId);
        Assert.Equal(model.GspProjectId, result.GspProjectId);
        Assert.Equal(model.ProgramName, result.ProgramName);
        Assert.Equal(model.ProgramYear, result.ProgramYear);

        Assert.Equal(model.StreetAddress, result.StreetAddress);
        Assert.Equal(model.City, result.City);

        //users
        Assert.Equal(model.StreetAddress, result.StreetAddress);
        Assert.Equal(model.City, result.City);
        Assert.Equal(model.State, result.State);
        Assert.Equal(model.ZipCode, result.ZipCode);

        Assert.Equal(model.ProjectPeriodStartDt, result.ProjectPeriodStartDt);
        Assert.Equal(model.ProjectPeriodEndDt, result.ProjectPeriodEndDt);
        Assert.Equal(model.EnrollmentStartDt, result.EnrollmentStartDt);
        Assert.Equal(model.EnrollmentEndDt, result.EnrollmentEndDt);

        Assert.Equal(model.OperatingSites.Count, result.OperatingSites.Count);
        Assert.All(result.OperatingSites.Zip(model.OperatingSites, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.OperatingSiteName, pair.mapped.OperatingSiteName);
                Assert.Equal(pair.source.ContactName, pair.mapped.ContactName);
                Assert.Equal(pair.source.EmailAddress, pair.mapped.EmailAddress);
                Assert.Equal(pair.source.PhoneNumber, pair.mapped.PhoneNumber);
                Assert.Equal(pair.source.StreetAddress, pair.mapped.StreetAddress);
                Assert.Equal(pair.source.StreetAddress2, pair.mapped.StreetAddress2);
                Assert.Equal(pair.source.City, pair.mapped.City);
                Assert.Equal(pair.source.State, pair.mapped.State);
                Assert.Equal(pair.source.ZipCode, pair.mapped.ZipCode);
                Assert.Equal(pair.source.Plus4, pair.mapped.Plus4);
                Assert.Equal(pair.source.InviteDate, pair.mapped.InviteDate);
                Assert.Equal(pair.source.InviteUserId, pair.mapped.InviteUserId);
            });

        Assert.Equal(model.SubGrantees.Count, result.SubGrantees.Count);
        Assert.All(result.SubGrantees.Zip(model.SubGrantees, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.GranteeCode, pair.mapped.GranteeCode);
                Assert.Equal(pair.source.GranteeName, pair.mapped.GranteeName);
                Assert.Equal(pair.source.Uei, pair.mapped.Uei);
                Assert.Equal(pair.source.StreetAddress, pair.mapped.StreetAddress);
                Assert.Equal(pair.source.City, pair.mapped.City);
                Assert.Equal(pair.source.State, pair.mapped.State);
                Assert.Equal(pair.source.ZipCode, pair.mapped.ZipCode);
            });

        Assert.Equal(model.ProjectType, result.ProjectType);
        Assert.Equal(model.Description, result.Description);
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesAccessResponse()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<Access>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.AccessName, result.AccessName);
        Assert.Equal(model.AccessLevel, result.AccessLevel);
        Assert.Equal(model.AccessType, result.AccessType);
        Assert.Equal(model.Description, result.Description);
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesRoleResponse()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<Role>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.RoleName, result.RoleName);
        Assert.Equal(model.FunctionalName, result.FunctionalName);
        Assert.Equal(model.Description, result.Description);
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesRoleResponseList()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<List<Role>>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.Count, result.Count);

        Assert.All(result.Zip(model, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.Id, pair.mapped.Id);
                Assert.Equal(pair.source.RoleName, pair.mapped.RoleName);
                Assert.Equal(pair.source.FunctionalName, pair.mapped.FunctionalName);
                Assert.Equal(pair.source.Description, pair.mapped.Description);
            });
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesAccessResponseList()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<List<Access>>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.Count, result.Count);

        Assert.All(result.Zip(model, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.Id, pair.mapped.Id);
                Assert.Equal(pair.source.AccessName, pair.mapped.AccessName);
                Assert.Equal(pair.source.AccessLevel, pair.mapped.AccessLevel);
                Assert.Equal(pair.source.AccessType, pair.mapped.AccessType);
                Assert.Equal(pair.source.Description, pair.mapped.Description);
            });
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesProjectResponseList()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<List<Project>>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.Count, result.Count);

        Assert.All(result.Zip(model, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.Id, pair.mapped.Id);
                Assert.Equal(pair.source.ProjectName, pair.mapped.ProjectName);
                Assert.Equal(pair.source.ProjectOrgCode, pair.mapped.ProjectOrgCode);
                Assert.Equal(pair.source.ProjectCode, pair.mapped.ProjectCode);
                Assert.Equal(pair.source.ProjectId, pair.mapped.ProjectId);
                Assert.Equal(pair.source.GspProjectId, pair.mapped.GspProjectId);
                Assert.Equal(pair.source.ProgramName, pair.mapped.ProgramName);
                Assert.Equal(pair.source.ProgramYear, pair.mapped.ProgramYear);
                Assert.Equal(pair.source.StreetAddress, pair.mapped.StreetAddress);
                Assert.Equal(pair.source.City, pair.mapped.City);
                Assert.Equal(pair.source.StreetAddress, pair.mapped.StreetAddress);
                Assert.Equal(pair.source.City, pair.mapped.City);
                Assert.Equal(pair.source.State, pair.mapped.State);
                Assert.Equal(pair.source.ZipCode, pair.mapped.ZipCode);
                Assert.Equal(pair.source.ProjectPeriodStartDt, pair.mapped.ProjectPeriodStartDt);
                Assert.Equal(pair.source.ProjectPeriodEndDt, pair.mapped.ProjectPeriodEndDt);
                Assert.Equal(pair.source.EnrollmentStartDt, pair.mapped.EnrollmentStartDt);
                Assert.Equal(pair.source.EnrollmentEndDt, pair.mapped.EnrollmentEndDt);
                Assert.Equal(pair.source.OperatingSites.Count, pair.mapped.OperatingSites.Count);
                Assert.Equal(pair.source.SubGrantees.Count, pair.mapped.SubGrantees.Count);
                Assert.Equal(pair.source.ProjectType, pair.mapped.ProjectType);
                Assert.Equal(pair.source.Description, pair.mapped.Description);
            });
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesOrganizationResponseList()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<List<Organization>>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.Count, result.Count);

        Assert.All(result.Zip(model, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.Id, pair.mapped.Id);
                Assert.Equal(pair.source.OrgName, pair.mapped.OrgName);
                Assert.Equal(pair.source.OrgCode, pair.mapped.OrgCode);
                Assert.Equal(pair.source.Description, pair.mapped.Description);
            });
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesUserList()
    {
        // Arrange
        var sut = Setup();

        var model = Fixture.Build<UserList>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.OrgCode, result.OrgCode);

        Assert.Equal(model.Users.Count, result.Users.Count);

        Assert.All(result.Users.Zip(model.Users, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.FirstName, pair.mapped.FirstName);
                Assert.Equal(pair.source.LastName, pair.mapped.LastName);
                Assert.Equal(pair.source.MiddleName, pair.mapped.MiddleName);
                Assert.Equal(pair.source.UserName, pair.mapped.UserName);
                Assert.Equal(pair.source.ExternalAccountId, pair.mapped.ExternalAccountId);

                Assert.Equal(pair.source.PreferredName, pair.mapped.PreferredName);
                Assert.Equal(pair.source.DateOfBirth, pair.mapped.DateOfBirth);
                Assert.Equal(pair.source.MiddleName, pair.mapped.MiddleName);
                Assert.Equal(pair.source.Pronouns, pair.mapped.Pronouns);
                Assert.Equal(pair.source.OrgCode, pair.mapped.OrgCode);
            });
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesOperatingSiteResponse()
    {
        var contact = Fixture.Build<User>()
              .Without(a => a.DateOfBirth)
              .Without(a => a.Education)
              .Without(a => a.Skills)
              .Without(a => a.MilitaryService)
              .Without(a => a.References)
              .Without(a => a.DocumentExpirationDate)
              .Without(a => a.DateOfBirths)
              .Create();

        var model = Fixture.Build<OperatingSite>()
              .With(o => o.Contact, contact)
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
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
    public void Map_CorrectlyMapsPropertiesAwardResponse()
    {
        var model = Fixture.Build<Award>()
            .Without(a => a.PerformanceStartDt)
            .Without(a => a.PerformanceEndDt)
            .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.AwardCode, result.AwardCode);
        Assert.Equal(model.AwardName, result.AwardName);
        Assert.Equal(model.GspListingNumber, result.GspListingNumber);
        Assert.Equal(model.Fain, result.Fain);
        Assert.Equal(model.Uei, result.Uei);
        Assert.Equal(model.PerformanceStartDt, result.PerformanceStartDt);
        Assert.Equal(model.PerformanceEndDt, result.PerformanceEndDt);
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesSubGranteeResponse()
    {
        var model = Fixture.Build<SubGrantee>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.GranteeCode, result.GranteeCode);
        Assert.Equal(model.GranteeName, result.GranteeName);
        Assert.Equal(model.Uei, result.Uei);
        Assert.Equal(model.StreetAddress, result.StreetAddress);
        Assert.Equal(model.City, result.City);
        Assert.Equal(model.State, result.State);
        Assert.Equal(model.ZipCode, result.ZipCode);
    }

    [Fact]
    public void Map_CorrectlyMapsPropertiesOperatingSiteResponseList()
    {
        var model = Fixture.Build<List<OperatingSite>>()
              .Create();

        IResponseMapper mapper = new ResponseMapper();

        // Act
        var result = mapper.Map(model);

        // Assert
        if (model == null)
        {
            Assert.Null(result);
            return;
        }

        Assert.NotNull(result);
        Assert.Equal(model.Count, result.Count);

        Assert.All(result.Zip(model, (mapped, source) => (mapped, source)),
            pair =>
            {
                Assert.Equal(pair.source.Id, pair.mapped.Id);
                Assert.Equal(pair.source.OperatingSiteName, pair.mapped.OperatingSiteName);
                Assert.Equal(pair.source.ContactName, pair.mapped.ContactName);
                Assert.Equal(pair.source.EmailAddress, pair.mapped.EmailAddress);
                Assert.Equal(pair.source.PhoneNumber, pair.mapped.PhoneNumber);
                Assert.Equal(pair.source.StreetAddress, pair.mapped.StreetAddress);
                Assert.Equal(pair.source.StreetAddress2, pair.mapped.StreetAddress2);
                Assert.Equal(pair.source.City, pair.mapped.City);
                Assert.Equal(pair.source.State, pair.mapped.State);
                Assert.Equal(pair.source.ZipCode, pair.mapped.ZipCode);
                Assert.Equal(pair.source.Plus4, pair.mapped.Plus4);
                Assert.Equal(pair.source.InviteDate, pair.mapped.InviteDate);
                Assert.Equal(pair.source.InviteUserId, pair.mapped.InviteUserId);
            });
    }

    private void TestUserCollectionResponseMapper(UserResponse result, User model)
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
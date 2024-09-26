using System.Data;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api.Services;

public interface IRequestMapper
{
    User Map(UserRequestModel requestModel);

    Role Map(RoleRequestModel roleRequestModel);

    SavedSearch Map(SavedSearchRequestModel requestModel);

    Collection Map(CollectionRequestModel requestModel);

    List<Collection> Map(CollectionListRequestModel requestModel);

    Reference Map(ReferenceRequestModel requestModel);

    Organization Map(OrganizationRequestModel requestModel);

    Project Map(ProjectRequestModel requestModel);

    UserRole Map(Role role);

    Access Map(AccessRequestModel requestModel);
}

public sealed class RequestMapper : IRequestMapper
{
    public User Map(UserRequestModel requestModel) => new()
    {
        FirstName = requestModel.FirstName,
        LastName = requestModel.LastName,
        MiddleName = requestModel.MiddleName,
        PreferredName = requestModel.PreferredName,
        UserName = requestModel.UserName,
        ExternalAccountId = requestModel.ExternalAccountId,
        DateOfBirth = requestModel.DateOfBirth,
        Pronouns = requestModel.Pronouns,
        Suffix = requestModel.Suffix,
        Prefix = requestModel.Prefix,
        OrgCode = requestModel.OrgCode,
        AccountStatus = requestModel.AccountStatus,
        EncryptedSocialSecurityNumber = requestModel.EncryptedSocialSecurityNumber,
        CountryOfBirth = requestModel.CountryOfBirth,
        CityOfBirth = requestModel.CityOfBirth,
        CitzenShipStatus = (Data.Core.Model.CitizenshipStatus)requestModel.CitzenShipStatus,

        Attributes = MapperUtils.MapList<AttributeRequestModel, AmeriCorps.Users.Data.Core.Attribute>(
            requestModel.Attributes,
            a => new AmeriCorps.Users.Data.Core.Attribute
            {
                Id = a.Id,
                Type = a.Type,
                Value = a.Value,
            }),

        Languages = MapperUtils.MapList<LanguageRequestModel, Language>(
            requestModel.Languages, l =>
                new Language
                {
                    Id = l.Id,
                    PickListId = l.PickListId,
                    IsPrimary = l.IsPrimary,
                    SpeakingAbility = l.SpeakingAbility,
                    WritingAbility = l.WritingAbility
                }),

        Addresses = MapperUtils.MapList<AddressRequestModel, Address>(
            requestModel.Addresses, a =>
                new Address
                {
                    Id = a.Id,
                    IsForeign = a.IsForeign,
                    Type = a.Type,
                    Street1 = a.Street1,
                    Street2 = a.Street2,
                    City = a.City,
                    State = a.State,
                    Country = a.Country,
                    ZipCode = a.ZipCode,
                    MovingWithinSixMonths = a.MovingWithinSixMonths
                }),

        Education = MapperUtils.MapList<EducationRequestModel, Education>(
            requestModel.Education, e =>
                new Education
                {
                    Id = e.Id,
                    Level = e.Level,
                    MajorAreaOfStudy = e.MajorAreaOfStudy,
                    Institution = e.Institution,
                    City = e.City,
                    State = e.State,
                    DateAttendedFrom = e.DateAttendedFrom,
                    DateAttendedTo = e.DateAttendedTo,
                    DegreeTypePursued = e.DegreeTypePursued,
                    DegreeCompleted = e.DegreeCompleted,
                }),

        Skills = MapperUtils.MapList<SkillRequestModel, Skill>(
            requestModel.Skills, s =>
                new Skill
                {
                    Id = s.Id,
                    PickListId = s.PickListId
                }),

        MilitaryService = MapperUtils.MapList<MilitaryServiceRequestModel, MilitaryService>(
            requestModel.MilitaryService, s =>
                new MilitaryService
                {
                    Id = s.Id,
                    PickListId = s.PickListId
                }),

        SavedSearches = MapperUtils.MapList<SavedSearchRequestModel, SavedSearch>(
            requestModel.SavedSearches, s =>
                new SavedSearch
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    Name = s.Name,
                    Filters = s.Filters,
                    NotificationsOn = s.NotificationsOn
                }),

        Relatives = MapperUtils.MapList<RelativeRequestModel, Relative>(
            requestModel.Relatives, r =>
                new Relative
                {
                    Id = r.Id,
                    Relationship = r.Relationship,
                    HighestEducationLevel = r.HighestEducationLevel,
                    AnnualIncome = r.AnnualIncome
                }),

        CommunicationMethods = MapperUtils.MapList<CommunicationMethodRequestModel,
            CommunicationMethod>(
            requestModel.CommunicationMethods, cm =>
                new CommunicationMethod
                {
                    Id = cm.Id,
                    Type = cm.Type,
                    Value = cm.Value,
                    IsPreferred = cm.IsPreferred
                }),

        Collection = MapperUtils.MapList<CollectionRequestModel, Collection>(
            requestModel.Collection, c =>
                new Collection()
                {
                    UserId = c.UserId,
                    Type = c.Type,
                    ListingId = c.ListingId,
                }),

        References = MapperUtils.MapList<ReferenceRequestModel, Reference>(
            requestModel.References, r =>
                new Reference
                {
                    TypeId = r.TypeId,
                    Relationship = r.Relationship,
                    RelationshipLength = r.RelationshipLength,
                    ContactName = r.ContactName,
                    Email = r.Email,
                    Phone = r.Phone,
                    Address = r.Address,
                    Company = r.Company,
                    Position = r.Position,
                    Notes = r.Notes,
                    CanContact = r.CanContact,
                    Contacted = r.Contacted,
                    DateContacted = r.DateContacted
                }),

        Roles = MapperUtils.MapList<UserRoleRequestModel, UserRole>(
            requestModel.UserRoles, c =>
                new UserRole()
                {
                    //Id = c.Id,
                    RoleName = c.RoleName,
                    FunctionalName = c.FunctionalName
                }),

        UserProjects = MapperUtils.MapList<UserProjectRequestModel, UserProject>(
            requestModel.UserProjects, p =>
                new UserProject()
                {
                    ProjectName = p.ProjectName,
                    ProjectCode = p.ProjectCode,
                    ProjectType = p.ProjectType,
                    ProjectOrg = p.ProjectOrg,
                    Active = p.Active,
                    ProjectRoles = Map(p.ProjectRoles),
                    ProjectAccess = Map(p.ProjectAccess)
                })
    };

    public Role Map(RoleRequestModel roleRequestModel) => new()
    {
        //Id = roleRequestModel.Id,
        RoleName = roleRequestModel.RoleName,
        FunctionalName = roleRequestModel.FunctionalName,
        Description = roleRequestModel.Description,
        RoleType = roleRequestModel.RoleType
    };

    public SavedSearch Map(SavedSearchRequestModel requestModel) => new()
    {
        Id = requestModel.Id,
        UserId = requestModel.UserId,
        Name = requestModel.Name,
        Filters = requestModel.Filters,
        NotificationsOn = requestModel.NotificationsOn
    };

    public Collection Map(CollectionRequestModel requestModel) => new()
    {
        UserId = requestModel.UserId,
        ListingId = requestModel.ListingId,
        Type = requestModel.Type
    };

    public List<Collection> Map(CollectionListRequestModel requestModel)
    {
        var collection = new List<Collection>();
        foreach (var listingId in requestModel.Listings)
        {
            collection.Add(new Collection()
            {
                UserId = requestModel.UserId,
                ListingId = listingId,
                Type = requestModel.Type
            });
        }

        return collection;
    }

    public Reference Map(ReferenceRequestModel requestModel) => new()
    {
        TypeId = requestModel.TypeId,
        Relationship = requestModel.Relationship,
        RelationshipLength = requestModel.RelationshipLength,
        ContactName = requestModel.ContactName,
        Email = requestModel.Email,
        Phone = requestModel.Phone,
        Address = requestModel.Address,
        Company = requestModel.Company,
        Position = requestModel.Position,
        Notes = requestModel.Notes,
        CanContact = requestModel.CanContact,
        Contacted = requestModel.Contacted,
        DateContacted = requestModel.DateContacted
    };

    public Organization Map(OrganizationRequestModel requestModel) => new()
    {
        OrgName = requestModel.OrgName,
        OrgCode = requestModel.OrgCode,
        Description = requestModel.Description
    };

    public Project Map(ProjectRequestModel requestModel) => new()
    {
        ProjectName = requestModel.ProjectName,
        ProjectCode = requestModel.ProjectCode,
        ProjectType = requestModel.ProjectType,
        ProjectOrg = requestModel.ProjectOrg,
        Description = requestModel.Description
    };

    public Access Map(AccessRequestModel requestModel) => new()
    {
        AccessName = requestModel.AccessName,
        AccessLevel = requestModel.AccessLevel,
        AccessType = requestModel.AccessType,
        Description = requestModel.Description
    };

    public UserRole Map(Role role) => new()
    {
        RoleName = role.RoleName,
        FunctionalName = role.FunctionalName
    };

    public List<ProjectRole> Map(List<ProjectRoleRequestModel> role) =>
       MapperUtils.MapList<ProjectRoleRequestModel, ProjectRole>(
                           role,
                           a => new ProjectRole
                           {
                               RoleName = a.RoleName,
                               FunctionalName = a.FunctionalName
                           });

    public List<ProjectAccess> Map(List<ProjectAccessRequestModel> access) =>
       MapperUtils.MapList<ProjectAccessRequestModel, ProjectAccess>(
                           access,
                           a => new ProjectAccess
                           {
                               AccessName = a.AccessName,
                               AccessLevel = a.AccessLevel
                           });
}
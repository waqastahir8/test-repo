using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api.Services;

public interface IResponseMapper
{
    UserResponse Map(User user);
    SavedSearchResponseModel Map(SavedSearch search);
    ReferenceResponseModel Map(Reference reference);
    List<SavedSearchResponseModel> Map(List<SavedSearch> searches);
    List<ReferenceResponseModel> Map(List<Reference> references);
}
public sealed class ResponseMapper : IResponseMapper
{
    public SavedSearchResponseModel Map(SavedSearch search) => new()
    {
        Id = search.Id,
        UserId = search.UserId,
        Name = search.Name,
        Filters = search.Filters,
        NotificationsOn = search.NotificationsOn
    };

    public List<SavedSearchResponseModel> Map(List<SavedSearch> searches) =>
        MapperUtils.MapList<AmeriCorps.Users.Data.Core.SavedSearch, SavedSearchResponseModel>(
                            searches,
                            a => new SavedSearchResponseModel
                            {
                                Id = a.Id,
                                UserId = a.UserId,
                                Name = a.Name,
                                Filters = a.Filters,
                                NotificationsOn = a.NotificationsOn
                            });

    public List<ReferenceResponseModel> Map(List<Reference> references) =>
        MapperUtils.MapList<AmeriCorps.Users.Data.Core.Reference, ReferenceResponseModel>(
                            references,
                            r => new ReferenceResponseModel
                            {
                                Id = r.Id,
                                UserId = r.UserId,
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
                            });

    public ReferenceResponseModel Map(Reference reference) => new()
    {
        Id = reference.Id,
        UserId = reference.UserId,
        TypeId = reference.TypeId,
        Relationship = reference.Relationship,
        RelationshipLength = reference.RelationshipLength,
        ContactName = reference.ContactName,
        Email = reference.Email,
        Phone = reference.Phone,
        Address = reference.Address,
        Company = reference.Company,
        Position = reference.Position,
        Notes = reference.Notes,
        CanContact = reference.CanContact,
        Contacted = reference.Contacted,
        DateContacted = reference.DateContacted
    };
    public UserResponse Map(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        MiddleName = user.MiddleName,
        PreferredName = user.PreferredName,
        UserName = user.UserName,
        ExternalAccountId = user.ExternalAccountId,
        DateOfBirth = user.DateOfBirth,

        Attributes = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Attribute, AttributeRequestModel>(
                        user.Attributes,
                        a => new AttributeRequestModel
                        {
                            Id = a.Id,
                            Type = a.Type,
                            Value = a.Value
                        }),


        Languages = MapperUtils.MapList<Language, LanguageRequestModel>(
                        user.Languages, l =>
                        new LanguageRequestModel
                        {
                            Id = l.Id,
                            PickListId = l.PickListId,
                            IsPrimary = l.IsPrimary,
                            SpeakingAbility = l.SpeakingAbility,
                            WritingAbility = l.WritingAbility
                        }),

        Addresses = MapperUtils.MapList<Address, AddressRequestModel>(
                        user.Addresses, a =>
                        new AddressRequestModel
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

        Education = MapperUtils.MapList<Education, EducationRequestModel>(
                        user.Education, e =>
                        new EducationRequestModel
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
                            DegreeCompleted = e.DegreeCompleted
                        }),

        Skills = MapperUtils.MapList<Skill, SkillRequestModel>(
                        user.Skills, s =>
                        new SkillRequestModel
                        {
                            Id = s.Id,
                            PickListId = s.PickListId
                        }),

        MilitaryService = MapperUtils.MapList<MilitaryService, MilitaryServiceRequestModel>(
                        user.MilitaryService, s =>
                        new MilitaryServiceRequestModel
                        {
                            Id = s.Id,
                            PickListId = s.PickListId
                        }),

        SavedSearches = MapperUtils.MapList<SavedSearch, SavedSearchRequestModel>(
                            user.SavedSearches, s => new SavedSearchRequestModel
                            {
                                Id = s.Id,
                                UserId = s.UserId,
                                Name = s.Name,
                                Filters = s.Filters,
                                NotificationsOn = s.NotificationsOn
                            }),

        Relatives = MapperUtils.MapList<Relative, RelativeRequestModel>(
                        user.Relatives, r =>
                        new RelativeRequestModel
                        {
                            Id = r.Id,
                            Relationship = r.Relationship,
                            HighestEducationLevel = r.HighestEducationLevel,
                            AnnualIncome = r.AnnualIncome
                        }),

        CommunicationMethods = MapperUtils.MapList<CommunicationMethod,
                                        CommunicationMethodRequestModel>(
                        user.CommunicationMethods, cm =>
                        new CommunicationMethodRequestModel
                        {
                            Id = cm.Id,
                            Type = cm.Type,
                            Value = cm.Value,
                            IsPreferred = cm.IsPreferred
                        })
    };
}
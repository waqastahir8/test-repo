using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api.Services;

public interface IRequestMapper
{
    User Map(UserRequestModel requestModel);
    SavedSearch Map(SavedSearchRequestModel requestModel);
    Reference Map(ReferenceRequestModel requestModel);
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

        Attributes = MapperUtils.MapList<AttributeRequestModel, AmeriCorps.Users.Data.Core.Attribute>(
                        requestModel.Attributes,
                        a => new AmeriCorps.Users.Data.Core.Attribute
                        {
                            Type = a.Type,
                            Value = a.Value
                        }),


        Languages = MapperUtils.MapList<LanguageRequestModel, Language>(
                        requestModel.Languages, l =>
                        new Language
                        {
                            PickListId = l.PickListId,
                            IsPrimary = l.IsPrimary,
                            SpeakingAbility = l.SpeakingAbility,
                            WritingAbility = l.WritingAbility
                        }),

        Addresses = MapperUtils.MapList<AddressRequestModel, Address>(
                        requestModel.Addresses, a =>
                        new Address
                        {
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

        Skills = MapperUtils.MapList<SkillRequestModel, Skill>(
                        requestModel.Skills, s =>
                        new Skill
                        {
                            PickListId = s.PickListId
                        }),

        MilitaryService = MapperUtils.MapList<MilitaryServiceRequestModel, MilitaryService>(
                        requestModel.MilitaryService, s =>
                        new MilitaryService
                        {
                            PickListId = s.PickListId
                        }),

        SavedSearches = MapperUtils.MapList<SavedSearchRequestModel, SavedSearch>(
                requestModel.SavedSearches, s =>
                new SavedSearch
                {
                    UserId = s.UserId,
                    Name = s.Name,
                    Filters = s.Filters,
                    NotificationsOn = s.NotificationsOn
                }),

        Relatives = MapperUtils.MapList<RelativeRequestModel, Relative>(
                        requestModel.Relatives, r =>
                        new Relative
                        {
                            Relationship = r.Relationship,
                            HighestEducationLevel = r.HighestEducationLevel,
                            AnnualIncome = r.AnnualIncome
                        }),

        CommunicationMethods = MapperUtils.MapList<CommunicationMethodRequestModel,
                                        CommunicationMethod>(
                        requestModel.CommunicationMethods, cm =>
                        new CommunicationMethod
                        {
                            Type = cm.Type,
                            Value = cm.Value,
                            IsPreferred = cm.IsPreferred
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
                        })

    };

    public SavedSearch Map(SavedSearchRequestModel requestModel) => new()
    {
        Id = requestModel.Id,
        UserId = requestModel.UserId,
        Name = requestModel.Name,
        Filters = requestModel.Filters,
        NotificationsOn = requestModel.NotificationsOn
    };

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
}
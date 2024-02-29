using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Services;

public interface IRequestMapper {
    User Map(UserRequestModel requestModel);
}
public sealed class RequestMapper : IRequestMapper
{
    public User Map(UserRequestModel requestModel) => new() {
        FirstName = requestModel.FirstName,
        LastName = requestModel.LastName,
        MiddleName = requestModel.MiddleName,
        PreferredName = requestModel.PreferredName,
        UserName = requestModel.UserName,
        DateOfBirth =  DateOnly.FromDateTime(requestModel.DateOfBirth),
 
        Attributes = MapperUtils.MapList<Attribute, AmeriCorps.Users.Data.Core.Attribute>(
                        requestModel.Attributes, 
                        a => new AmeriCorps.Users.Data.Core.Attribute { 
                                        Type = a.Type, Value = a.Value}),


        Languages = MapperUtils.MapList<Language, AmeriCorps.Users.Data.Core.Language>(
                        requestModel.Languages, l =>
                        new AmeriCorps.Users.Data.Core.Language {
                                                PickListId = l.PickListId, 
                                                IsPrimary = l.IsPrimary, 
                                                SpeakingAbility = l.SpeakingAbility, 
                                                WritingAbility = l.WritingAbility}),

        Addresses = MapperUtils.MapList<Address, AmeriCorps.Users.Data.Core.Address>(
                        requestModel.Addresses, a =>
                        new AmeriCorps.Users.Data.Core.Address {
                                                IsForeign = a.IsForeign,
                                                Type = a.Type,
                                                Street1 = a.Street1,
                                                Street2 = a.Street2,
                                                City = a.City,
                                                State = a.State,
                                                Country = a.Country,
                                                ZipCode = a.ZipCode,
                                                MovingWithinSixMonths = a.MovingWithinSixMonths}),

        Education = MapperUtils.MapList<Education, AmeriCorps.Users.Data.Core.Education>(
                        requestModel.Education, e =>
                        new AmeriCorps.Users.Data.Core.Education {
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

        Skills = MapperUtils.MapList<Skill, AmeriCorps.Users.Data.Core.Skill>(
                        requestModel.Skills, s =>
                        new AmeriCorps.Users.Data.Core.Skill {
                                PickListId = s.PickListId
                        }),
                
        Relatives = MapperUtils.MapList<Relative, AmeriCorps.Users.Data.Core.Relative>(
                        requestModel.Relatives, r => 
                        new AmeriCorps.Users.Data.Core.Relative {
                                Relationship = r.Relationship,
                                HighestEducationLevel = r.HighestEducationLevel,
                                AnnualIncome = r.AnnualIncome
                        }),
        
        CommunicationMethods = MapperUtils.MapList<CommunicationMethod,
                                        AmeriCorps.Users.Data.Core.CommunicationMethod>(
                        requestModel.CommunicationMethods, cm => 
                        new AmeriCorps.Users.Data.Core.CommunicationMethod {
                                Type = cm.Type,
                                Value = cm.Value, 
                                IsPreferred = cm.IsPreferred
                        })
            
    };      
}
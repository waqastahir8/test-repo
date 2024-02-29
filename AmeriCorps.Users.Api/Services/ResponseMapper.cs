using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Services;

public interface IResponseMapper {
    UserResponseModel Map(User user);
}
public sealed class ResponseMapper : IResponseMapper
{
    public UserResponseModel Map(User user) => new() {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        MiddleName = user.MiddleName,
        PreferredName = user.PreferredName,
        UserName = user.UserName,
        DateOfBirth = user.DateOfBirth,

        Attributes = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Attribute, AmeriCorps.Users.Api.Attribute>(
                        user.Attributes, 
                        a => new AmeriCorps.Users.Api.Attribute { 
                                        Id = a.Id,
                                        Type = a.Type, Value = a.Value}),


        Languages = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Language, AmeriCorps.Users.Api.Language>(
                        user.Languages, l =>
                        new AmeriCorps.Users.Api.Language {
                                                Id = l.Id,
                                                PickListId = l.PickListId, 
                                                IsPrimary = l.IsPrimary, 
                                                SpeakingAbility = l.SpeakingAbility, 
                                                WritingAbility = l.WritingAbility}),

        Addresses = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Address, AmeriCorps.Users.Api.Address>(
                        user.Addresses, a =>
                        new AmeriCorps.Users.Api.Address {
                                                Id = a.Id,
                                                IsForeign = a.IsForeign,
                                                Type = a.Type,
                                                Street1 = a.Street1,
                                                Street2 = a.Street2,
                                                City = a.City,
                                                State = a.State,
                                                Country = a.Country,
                                                ZipCode = a.ZipCode,
                                                MovingWithinSixMonths = a.MovingWithinSixMonths}),

        Education = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Education, AmeriCorps.Users.Api.Education>(
                        user.Education, e =>
                        new AmeriCorps.Users.Api.Education {
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

        Skills = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Skill, AmeriCorps.Users.Api.Skill>(
                        user.Skills, s =>
                        new AmeriCorps.Users.Api.Skill {
                                Id = s.Id,
                                PickListId = s.PickListId
                        }),
                
        Relatives = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Relative, AmeriCorps.Users.Api.Relative>(
                        user.Relatives, r => 
                        new AmeriCorps.Users.Api.Relative {
                                Id = r.Id,
                                Relationship = r.Relationship,
                                HighestEducationLevel = r.HighestEducationLevel,
                                AnnualIncome = r.AnnualIncome
                        }),
        
        CommunicationMethods = MapperUtils.MapList<AmeriCorps.Users.Data.Core.CommunicationMethod,
                                        AmeriCorps.Users.Api.CommunicationMethod>(
                        user.CommunicationMethods, cm => 
                        new AmeriCorps.Users.Api.CommunicationMethod {
                                Id = cm.Id,
                                Type = cm.Type,
                                Value = cm.Value, 
                                IsPreferred = cm.IsPreferred
                        })          
    };      
}
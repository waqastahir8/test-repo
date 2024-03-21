using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api.Services;

public interface IResponseMapper {
    UserResponse Map(User user);
}
public sealed class ResponseMapper : IResponseMapper
{
    public UserResponse Map(User user) => new() {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        MiddleName = user.MiddleName,
        PreferredName = user.PreferredName,
        UserName = user.UserName,
        DateOfBirth = user.DateOfBirth,

        Attributes = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Attribute, AttributeRequestModel>(
                        user.Attributes, 
                        a => new AttributeRequestModel { 
                                        Id = a.Id,
                                        Type = a.Type, Value = a.Value}),


        Languages = MapperUtils.MapList<Language, LanguageRequestModel>(
                        user.Languages, l =>
                        new LanguageRequestModel {
                                                Id = l.Id,
                                                PickListId = l.PickListId, 
                                                IsPrimary = l.IsPrimary, 
                                                SpeakingAbility = l.SpeakingAbility, 
                                                WritingAbility = l.WritingAbility}),

        Addresses = MapperUtils.MapList<Address, AddressRequestModel>(
                        user.Addresses, a =>
                        new AddressRequestModel {
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

        Education = MapperUtils.MapList<Education, EducationRequestModel>(
                        user.Education, e =>
                        new EducationRequestModel {
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
                        new SkillRequestModel {
                                Id = s.Id,
                                PickListId = s.PickListId
                        }),
                
        Relatives = MapperUtils.MapList<Relative, RelativeRequestModel>(
                        user.Relatives, r => 
                        new RelativeRequestModel {
                                Id = r.Id,
                                Relationship = r.Relationship,
                                HighestEducationLevel = r.HighestEducationLevel,
                                AnnualIncome = r.AnnualIncome
                        }),
        
        CommunicationMethods = MapperUtils.MapList<CommunicationMethod,
                                        CommunicationMethodRequestModel>(
                        user.CommunicationMethods, cm => 
                        new CommunicationMethodRequestModel {
                                Id = cm.Id,
                                Type = cm.Type,
                                Value = cm.Value, 
                                IsPreferred = cm.IsPreferred
                        })          
    };      
}
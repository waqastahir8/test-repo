using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Api.Models;

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

        Attributes = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Attribute, AttributeDTO>(
                        user.Attributes, 
                        a => new AttributeDTO { 
                                        Id = a.Id,
                                        Type = a.Type, Value = a.Value}),


        Languages = MapperUtils.MapList<Language, LanguageDTO>(
                        user.Languages, l =>
                        new LanguageDTO {
                                                Id = l.Id,
                                                PickListId = l.PickListId, 
                                                IsPrimary = l.IsPrimary, 
                                                SpeakingAbility = l.SpeakingAbility, 
                                                WritingAbility = l.WritingAbility}),

        Addresses = MapperUtils.MapList<Address, AddressDTO>(
                        user.Addresses, a =>
                        new AddressDTO {
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

        Education = MapperUtils.MapList<Education, EducationDTO>(
                        user.Education, e =>
                        new EducationDTO {
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

        Skills = MapperUtils.MapList<Skill, SkillDTO>(
                        user.Skills, s =>
                        new SkillDTO {
                                Id = s.Id,
                                PickListId = s.PickListId
                        }),
                
        Relatives = MapperUtils.MapList<Relative, RelativeDTO>(
                        user.Relatives, r => 
                        new RelativeDTO {
                                Id = r.Id,
                                Relationship = r.Relationship,
                                HighestEducationLevel = r.HighestEducationLevel,
                                AnnualIncome = r.AnnualIncome
                        }),
        
        CommunicationMethods = MapperUtils.MapList<CommunicationMethod,
                                        CommunicationMethodDTO>(
                        user.CommunicationMethods, cm => 
                        new CommunicationMethodDTO {
                                Id = cm.Id,
                                Type = cm.Type,
                                Value = cm.Value, 
                                IsPreferred = cm.IsPreferred
                        })          
    };      
}
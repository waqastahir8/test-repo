using Xunit;
using AmeriCorps.Users.Api.Services;

namespace AmeriCorps.Users.Api.Tests;

public sealed class RequestMapperTests
{
    [Fact]
    public void Map_CorrectlyMapsProperties()
    {
        // Arrange
        var requestModel = new UserRequestModel
        {
            FirstName = "John",
            LastName = "Doe",
            MiddleName = "Middle",
            PreferredName = "Preferred",
            UserName = "johndoe",
            DateOfBirth = new DateTime(2000, 1, 1),
            Attributes = new List<Attribute>(){
                            new Attribute{ Type = "type1", Value = "value1"},
                            new Attribute{ Type = "type2", Value = "value2"}},

            Languages = new List<Language>(){
                            new Language { PickListId = "english", IsPrimary = true, 
                                           SpeakingAbility = "proficient", WritingAbility = "proficient"},   
                            new Language { PickListId = "spanish", IsPrimary = false, 
                                           SpeakingAbility = "basic", WritingAbility = "basic"}},

            Addresses = new List<Address>() {
                            new Address { IsForeign = false, Type = "permanent", Street1 = "123 Main Rd", 
                                          Street2 = "Apt 16", City = "City", State = "State", Country = "US", ZipCode = "11111", MovingWithinSixMonths = true},
                            new Address { IsForeign = true, Type = "mailing", Street1 = "123 El Camino", 
                                          Street2 = "Apt 16", City = "City", State = "State", Country = "Mexico", ZipCode = "11111", MovingWithinSixMonths = false}},

            Education = new List<Education>() {
                            new Education { Level = "college", MajorAreaOfStudy = "basket weaving", 
                                            Institution = "the ohio state university",City = "city",State = "state",
                                            DateAttendedFrom = new DateOnly(2000,1,1),DateAttendedTo = new DateOnly(2005,1,1),
                                            DegreeTypePursued = "bs", DegreeCompleted = true},         
                            new Education { Level = "college", MajorAreaOfStudy = "basket weaving", 
                                            Institution = "michigan state",City = "city",State = "state",
                                            DateAttendedFrom = new DateOnly(2000,1,1),DateAttendedTo = new DateOnly(2005,1,1),
                                            DegreeTypePursued = "bs", DegreeCompleted = true}},

            Skills = new List<Skill>() {
                            new Skill { PickListId = "skill1"},
                            new Skill { PickListId = "skill2"}},

            Relatives = new List<Relative>() {
                            new Relative { Relationship = "spouse", HighestEducationLevel = "college", AnnualIncome = 35000},
                            new Relative { Relationship = "mother", HighestEducationLevel = "highschool", AnnualIncome = 65000}},
            
            //TODO:  Use Fixtures
            CommunicationMethods = new List<CommunicationMethod>() {
                            new CommunicationMethod { Type = "email", Value = "test@gmail.com", IsPreferred = true},
                            new CommunicationMethod { Type = "phone", Value = "9154344334", IsPreferred = false}}

        };

        
        IRequestMapper mapper = new RequestMapper(); // Assuming this is the class containing the Map method

        // Act
        var result = mapper.Map(requestModel);

        // Assert
        Assert.Equal(requestModel.FirstName, result.FirstName);
        Assert.Equal(requestModel.LastName, result.LastName);
        Assert.Equal(requestModel.MiddleName, result.MiddleName);
        Assert.Equal(requestModel.PreferredName, result.PreferredName);
        Assert.Equal(requestModel.UserName, result.UserName);
        Assert.Equal(DateOnly.FromDateTime(requestModel.DateOfBirth), result.DateOfBirth);

        //Assert attributes
        Assert.Equal(2, result.Attributes.Count);  
        
        Assert.All(result.Attributes.Zip(requestModel.Attributes, (mapped, source) => (mapped, source)),
            pair => {
                        Assert.Equal(pair.source.Type, pair.mapped.Type);
                        Assert.Equal(pair.source.Value, pair.mapped.Value);
            });
        
        //Assert languages
        Assert.Equal(2, result.Languages.Count);  
        
        Assert.All(result.Languages.Zip(requestModel.Languages, (mapped, source) => (mapped, source)),
            pair => {
                        Assert.Equal(pair.source.PickListId, pair.mapped.PickListId);
                        Assert.Equal(pair.source.IsPrimary, pair.mapped.IsPrimary);
                        Assert.Equal(pair.source.SpeakingAbility, pair.mapped.SpeakingAbility);
                        Assert.Equal(pair.source.WritingAbility, pair.mapped.WritingAbility);
            });
        
        //Assert addresses
        Assert.Equal(2, result.Addresses.Count);
        Assert.All(result.Addresses.Zip(requestModel.Addresses, (mapped, source) => (mapped, source)),
        pair => {
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
        Assert.Equal(2, result.Education.Count);
        Assert.All(result.Education.Zip(requestModel.Education, (mapped, source) => (mapped, source)),
        pair => {
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
        Assert.Equal(2, result.Skills.Count);  
        
        Assert.All(result.Skills.Zip(requestModel.Skills, (mapped, source) => (mapped, source)),
            pair => {
                        Assert.Equal(pair.source.PickListId, pair.mapped.PickListId);
            });

        //Assert Relatives
        Assert.Equal(2, result.Relatives.Count);  
        
        Assert.All(result.Relatives.Zip(requestModel.Relatives, (mapped, source) => (mapped, source)),
            pair => {
                        Assert.Equal(pair.source.Relationship, pair.mapped.Relationship);
                        Assert.Equal(pair.source.HighestEducationLevel, pair.mapped.HighestEducationLevel);
                        Assert.Equal(pair.source.AnnualIncome, pair.mapped.AnnualIncome);
            });
        
        //Assert Communication Methods
        Assert.Equal(2, result.CommunicationMethods.Count);  
        
        Assert.All(result.CommunicationMethods.Zip(requestModel.CommunicationMethods, (mapped, source) => (mapped, source)),
            pair => {
                        Assert.Equal(pair.source.Type, pair.mapped.Type);
                        Assert.Equal(pair.source.Value, pair.mapped.Value);
                        Assert.Equal(pair.source.IsPreferred, pair.mapped.IsPreferred);
            });
    }
}
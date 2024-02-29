using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Api;

public sealed class UserRequestModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string PreferredName{ get; set; } = string.Empty;
    public string UserName {get; set; } = string.Empty;
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; } 
    public List<Attribute> Attributes { get; set; } = new List<Attribute>();
    public List<Language> Languages { get; set; } = new List<Language>();
    public List<Address> Addresses { get; set; } = new List<Address>();    
    public List<Education> Education { get; set; } = new List<Education>();
    public List<Skill> Skills { get; set; } = new List<Skill>();
    public List<Relative> Relatives { get; set; } = new List<Relative>();
    public List<CommunicationMethod> CommunicationMethods { get; set; } = new List<CommunicationMethod>();
}
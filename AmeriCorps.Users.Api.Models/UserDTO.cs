using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Api.Models;

public class UserDTO
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string PreferredName{ get; set; } = string.Empty;
    public string UserName {get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; } 
    public List<AttributeDTO> Attributes { get; set; } = new List<AttributeDTO>();
    public List<LanguageDTO> Languages { get; set; } = new List<LanguageDTO>();
    public List<AddressDTO> Addresses { get; set; } = new List<AddressDTO>();    
    public List<EducationDTO> Education { get; set; } = new List<EducationDTO>();
    public List<SkillDTO> Skills { get; set; } = new List<SkillDTO>();
    public List<RelativeDTO> Relatives { get; set; } = new List<RelativeDTO>();
    public List<CommunicationMethodDTO> CommunicationMethods { get; set; } = new List<CommunicationMethodDTO>();
}
using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public class UserRequestModel
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string PreferredName{ get; set; } = string.Empty;
    public string UserName {get; set; } = string.Empty;
    public string ExternalAccountId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Prefix { get; set; } = string.Empty;
    public string? Suffix { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Pronouns { get; set; } = string.Empty;
    public string OrgCode { get; set; } = string.Empty;
    public List<AttributeRequestModel> Attributes { get; set; } = new List<AttributeRequestModel>();
    public List<LanguageRequestModel> Languages { get; set; } = new List<LanguageRequestModel>();
    public List<AddressRequestModel> Addresses { get; set; } = new List<AddressRequestModel>();
    public List<EducationRequestModel> Education { get; set; } = new List<EducationRequestModel>();
    public List<SkillRequestModel> Skills { get; set; } = new List<SkillRequestModel>();
    public List<MilitaryServiceRequestModel> MilitaryService { get; set; } = new List<MilitaryServiceRequestModel>();
    public List<SavedSearchRequestModel> SavedSearches { get; set; } = new List<SavedSearchRequestModel>();
    public List<RelativeRequestModel> Relatives { get; set; } = new List<RelativeRequestModel>();
    public List<CommunicationMethodRequestModel> CommunicationMethods { get; set; } = new List<CommunicationMethodRequestModel>();
    public List<ReferenceRequestModel> References { get; set; } = new List<ReferenceRequestModel>();
    public List<CollectionRequestModel> Collection { get; set; } = new List<CollectionRequestModel>();
    public List<RoleRequestModel> Roles { get; set; } = new List<RoleRequestModel>();
    public List<UserProjectRequestModel> UserProjects { get; set; } = new List<UserProjectRequestModel>();
}

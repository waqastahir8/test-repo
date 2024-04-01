using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Data.Core;

public sealed class User
{
    public int Id { get; set; }
    public bool Searchable { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string ExternalAccountId { get; set; } = string.Empty;
    public string? Prefix { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string PreferredName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public List<Attribute> Attributes { get; set; } = new List<Attribute>();
    public List<Language> Languages { get; set; } = new List<Language>();
    public List<Address> Addresses { get; set; } = new List<Address>();
    public List<Education> Education { get; set; } = new List<Education>();
    public List<Skill> Skills { get; set; } = new List<Skill>();
    public List<MilitaryService> MilitaryService { get; set; } = new List<MilitaryService>();
    public List<SavedSearch> SavedSearches { get; set; } = new List<SavedSearch>();
    public List<Relative> Relatives { get; set; } = new List<Relative>();
    public List<CommunicationMethod> CommunicationMethods { get; set; } = new List<CommunicationMethod>();
}
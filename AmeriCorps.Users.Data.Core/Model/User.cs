using System.ComponentModel.DataAnnotations.Schema;

namespace AmeriCorps.Users.Data.Core;

public sealed class User
{
    public int Id { get; set; }
    public bool Searchable { get; set; }
    public required BasicInfo BasicInfo { get; set; }
    public About? About { get; set; }
     public List<Language> Languages { get; set; } = new List<Language>();
    public List<Address> Addresses { get; set; } = new List<Address>();
    public List<Education> Education { get; set; } = new List<Education>();
     public List<Skill> Skills { get; set; } = new List<Skill>();
    public List<Relative> Relatives { get; set; } = new List<Relative>();
    public required List<CommunicationMethod> CommunicationMethods { get; set; } 
}
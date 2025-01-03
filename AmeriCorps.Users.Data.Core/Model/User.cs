﻿using AmeriCorps.Users.Data.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data.Core;

[Index(nameof(OrgCode), nameof(UserAccountStatus), nameof(EncryptedSocialSecurityNumber))]
public sealed class User : Entity
{
    public bool Searchable { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string ExternalAccountId { get; set; } = string.Empty;
    public string? Prefix { get; set; }
    public string? Suffix { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string PreferredName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public List<DateOfBirth>? DateOfBirths { get; set; } = new List<DateOfBirth>();
    public string Pronouns { get; set; } = string.Empty;
    public List<Attribute> Attributes { get; set; } = new List<Attribute>();
    public List<Language> Languages { get; set; } = new List<Language>();
    public List<Address> Addresses { get; set; } = new List<Address>();
    public List<Education> Education { get; set; } = new List<Education>();
    public List<Skill> Skills { get; set; } = new List<Skill>();
    public List<MilitaryService> MilitaryService { get; set; } = new List<MilitaryService>();
    public List<SavedSearch> SavedSearches { get; set; } = new List<SavedSearch>();
    public List<Relative> Relatives { get; set; } = new List<Relative>();
    public List<CommunicationMethod> CommunicationMethods { get; set; } = new List<CommunicationMethod>();
    public List<Reference> References { get; set; } = new List<Reference>();
    public List<Collection> Collection { get; set; } = new();
    public List<UserRole> Roles { get; set; } = new List<UserRole>();
    public List<UserProject> UserProjects { get; set; } = new List<UserProject>();
    public string OrgCode { get; set; } = string.Empty;
    public string EncryptedSocialSecurityNumber { get; set; } = string.Empty;
    public List<EncryptedSocialSecurityNumber> EncryptedSocialSecurityNumbers { get; set; } = new List<EncryptedSocialSecurityNumber>();
    public CitizenshipStatus CitzenShipStatus { get; set; }
    public List<CountryOfBirth> CountryOfBirth { get; set; } = new List<CountryOfBirth>();
    public List<StateOfBirth> StateOfBirth { get; set; } = new List<StateOfBirth>();
    public List<CityOfBirth> CityOfBirth { get; set; } = new List<CityOfBirth>();
    public string? ResidentRegistrationNumber { get; set; } = string.Empty;
    public DateOnly? DocumentExpirationDate { get; set; }
    public UserAccountStatus UserAccountStatus { get; set; }
    public DateTime? InviteDate { get; set; }
    public int InviteUserId { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public List<DirectDeposit> DirectDeposits { get; set; } = new List<DirectDeposit>();
    public List<TaxWithHolding> TaxWithHoldings { get; set; } = new List<TaxWithHolding>();
    public SocialSecurityVerification? SocialSecurityVerification { get; set; }
    public string? PPIUpdateNote { get; set; } = string.Empty;
}
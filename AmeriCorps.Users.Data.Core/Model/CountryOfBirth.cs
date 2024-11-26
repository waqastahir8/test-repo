namespace AmeriCorps.Users.Data.Core.Model;

public sealed class CountryOfBirth : EntityWithUserId
{
    public string? BirthCountry { get; set; } = string.Empty;
}


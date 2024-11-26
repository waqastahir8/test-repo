namespace AmeriCorps.Users.Data.Core.Model;

public sealed class CityOfBirth : EntityWithUserId
{
    public string? BirthCity { get; set; } = string.Empty;
}


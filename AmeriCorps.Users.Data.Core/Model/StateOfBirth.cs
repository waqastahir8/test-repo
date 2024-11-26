namespace AmeriCorps.Users.Data.Core.Model;

public sealed class StateOfBirth : EntityWithUserId
{
    public string? BirthState { get; set; } = string.Empty;
}


namespace AmeriCorps.Users.Data.Core.Model;

public sealed class DateOfBirth : EntityWithUserId
{
    public DateOnly? BirthDate { get; set; }
}


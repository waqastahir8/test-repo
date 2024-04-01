using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api;

public interface IValidator {
    bool Validate(UserRequestModel model);
    bool Validate(SavedSearchRequestModel model);
}

public sealed class Validator : IValidator 
{
    public bool Validate(UserRequestModel model) =>
        !string.IsNullOrWhiteSpace(model.FirstName) &&
        !string.IsNullOrWhiteSpace(model.LastName) &&
        !string.IsNullOrWhiteSpace(model.UserName) &&
        IsOver18(model.DateOfBirth);

    public bool Validate(SavedSearchRequestModel model) =>
        !string.IsNullOrWhiteSpace(model.Name) &&
        !string.IsNullOrWhiteSpace(model.Filters) &&
        model.UserId == default;
    private bool IsOver18(DateOnly dateOfBirth)
    {
        var startDate = dateOfBirth;
        var endDate = DateTime.Now;
        int years = endDate.Year - startDate.Year;

        // Check if the endDate's month and day are before the startDate's month and day
        if (endDate.Month < startDate.Month || (endDate.Month == startDate.Month && endDate.Day < startDate.Day))
        {
            years--;
        }
        return years >= 18;
    }
}
using AmeriCorps.Users.Api.Helpers.Collection;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api;

public interface IValidator
{
    bool Validate(UserRequestModel model);
    bool Validate(SavedSearchRequestModel model);
    bool Validate(ReferenceRequestModel model);

    ValidationResponse? Validate(CollectionRequestModel model);

    // bool Validate(CollectionRequestModel model);
}

public sealed class Validator : IValidator
{
    public bool Validate(UserRequestModel model) =>
        !string.IsNullOrWhiteSpace(model.FirstName) &&
        !string.IsNullOrWhiteSpace(model.LastName) &&
        !string.IsNullOrWhiteSpace(model.UserName) &&
        !string.IsNullOrWhiteSpace(model.ExternalAccountId) &&
        IsOver18(model.DateOfBirth);

    public bool Validate(SavedSearchRequestModel model) =>
        !string.IsNullOrWhiteSpace(model.Name) &&
        !string.IsNullOrWhiteSpace(model.Filters);


    public bool Validate(ReferenceRequestModel model) =>
        !string.IsNullOrWhiteSpace(model.TypeId) &&
        !string.IsNullOrWhiteSpace(model.ContactName) &&
        !string.IsNullOrWhiteSpace(model.Email) &&
        !string.IsNullOrWhiteSpace(model.Phone);


    public ValidationResponse? Validate(CollectionRequestModel model)
    {
        var validationResponse = new ValidationResponse();
        var isValidType = CollectionHelpers.ContainsType(model.Type);
        if (!isValidType)
        {
            validationResponse.IsValid = false;
            validationResponse.ValidationMessage = "Invalid collection type";
        }
           

        return validationResponse;

    }
    
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

public class ValidationResponse
{
    public bool IsValid { get; set; } = true;

    public string ValidationMessage { get; set; } = string.Empty;

}
using AmeriCorps.Users.Api.Helpers.Collection;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api;

public interface IValidator
{
    bool Validate(UserRequestModel model);

    bool Validate(RoleRequestModel model);

    bool Validate(SavedSearchRequestModel model);
    bool Validate(ReferenceRequestModel model);

    ValidationResponse? Validate(CollectionRequestModel model);
}

public sealed class Validator : IValidator
{
    public bool Validate(UserRequestModel model) =>
        !string.IsNullOrWhiteSpace(model.FirstName) &&
        !string.IsNullOrWhiteSpace(model.LastName);

    public bool Validate(RoleRequestModel model) =>
        !string.IsNullOrWhiteSpace(model.RoleName) &&
        !string.IsNullOrWhiteSpace(model.FucntionalName) &&
        !string.IsNullOrWhiteSpace(model.Description);

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
}

public class ValidationResponse
{
    public bool IsValid { get; set; } = true;

    public string ValidationMessage { get; set; } = string.Empty;

}
using FluentValidation;
using AmeriCorps.Users.Models;
namespace AmeriCorps.Users.Api;


public sealed class SearchRequestValidator : AbstractValidator<SavedSearchRequestModel>
{
    public SearchRequestValidator()
    {
        RuleFor(search => search.Name).NotEmpty();
        RuleFor(search => search.Filters).NotEmpty();
    }
}
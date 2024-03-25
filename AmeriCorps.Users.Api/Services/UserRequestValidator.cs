using FluentValidation;
using AmeriCorps.Users.Models;

namespace AmeriCorps.Users.Api;

public sealed class UserRequestValidator : AbstractValidator<UserRequestModel>
{
    public UserRequestValidator()
    {
        RuleFor(user => user.LastName).NotEmpty();
        RuleFor(user => user.FirstName).NotEmpty();
        RuleFor(user => user.UserName).NotEmpty();
        RuleFor(user => user.DateOfBirth).Must(BeOver18);
    }

    private bool BeOver18(DateOnly dateOfBirth)
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
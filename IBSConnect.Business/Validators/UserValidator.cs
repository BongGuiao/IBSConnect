using FluentValidation;
using IBSConnect.Business.Models;

namespace IBSConnect.Business.Validators;

public class UserValidator : AbstractValidator<UserViewModel>
{
    public UserValidator(bool validatePassword)
    {
        RuleFor(user => user.Username)
            .NotEmpty().NotNull()
            .WithMessage("The user name cannot be empty");

        RuleFor(user => user.Username)
            .MinimumLength(5)
            .WithMessage("The user name must be at least 5 characters in length");

        if (validatePassword)
        {
            RuleFor(user => user.Password)
                .MinimumLength(8)
                .WithMessage("The password must be at least 5 characters in length");
        }

        RuleFor(user => user.FirstName)
            .NotEmpty().NotNull()
            .WithMessage("First name cannot be empty");

        RuleFor(user => user.LastName)
            .NotEmpty().NotNull()
            .WithMessage("Last name cannot be empty");
    }
}
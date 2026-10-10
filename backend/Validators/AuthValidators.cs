using backend.Models;
using FluentValidation;

public class RegisterValidator : AbstractValidator<User>
{
	public RegisterValidator()
	{
		RuleFor(user => user.Name)
			.NotEmpty().WithMessage("Name can not be Empty")
			.Length(100);

		RuleFor(user => user.Email)
			.NotEmpty().WithMessage("Email can not be Empty")
			.EmailAddress().WithMessage("It is not email address");

		RuleFor(user => user.Password)
			.NotEmpty().WithMessage("Password can not be Empty")
			.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$")
			.WithMessage("The password must contain at least 8 characters, including uppercase and lowercase letters, a digit, and a special character.");
	}
}
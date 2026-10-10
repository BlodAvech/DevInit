using backend.Models.DTOs;
using FluentValidation;

namespace backend.Validators
{
	public class RegisterValidator : AbstractValidator<UserRegisterDTO>
	{
		public RegisterValidator()
		{
			RuleFor(user => user.Name)
				.NotEmpty().WithMessage("Name can not be Empty")
				.MaximumLength(100);

			RuleFor(user => user.Email)
				.NotEmpty().WithMessage("Email can not be Empty")
				.EmailAddress().WithMessage("It is not email address")
				.MaximumLength(200);

			RuleFor(user => user.Password)
				.NotEmpty().WithMessage("Password can not be Empty")
				.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$")
				.WithMessage("The password must contain at least 8 characters, including uppercase and lowercase letters, a digit, and a special character.")
				.MaximumLength(200);
		}
	}
	public class LoginValidator : AbstractValidator<UserLoginDTO>
	{
		public LoginValidator()
		{
			RuleFor(user => user.Email)
				.NotNull().WithMessage("Email can not be Empty")
				.EmailAddress().WithMessage("It is not email address")
				.MaximumLength(200);

			RuleFor(user => user.Password)
				.NotNull().WithMessage("Password can not be Empty")
				.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{8,}$")
				.WithMessage("The password must contain at least 8 characters, including uppercase and lowercase letters, a digit, and a special character.")
				.MaximumLength(200);
		}
	}
}
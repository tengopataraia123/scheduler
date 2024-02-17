using FluentValidation;

namespace ProgramServer.Application.DTOs
{
    public class UserCreateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
    }

    public class UserCreateModelValidator : AbstractValidator<UserCreateModel>
    {
        public UserCreateModelValidator()
        {

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Enter the first name");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Enter the last name");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Enter the email");

        }

    }
}


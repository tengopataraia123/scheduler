using System;
using FluentValidation;
using ProgramServer.Domain.Roles;
using ProgramServer.Domain.SubjectUsers;

namespace ProgramServer.Application.DTOs
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public RoleModel Role { get; set; }
        public string MacAddressUser { get; set; }
    }

    public class UserValidator : AbstractValidator<UserModel>
    {
        public UserValidator()
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


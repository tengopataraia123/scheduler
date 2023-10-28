using MainServer.Application.Common;
using MainServer.Domain.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.RegularExpressions;

namespace MainServer.Application.Services.Users.Models
{
    public class UserRegistrationModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserRegistrationModelValidator : AbstractValidator<UserRegistrationModel>
    {
        public UserRegistrationModelValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("Name is Empty, please enter the name")
                .Length(1, 50).WithMessage("Name length isn't correct");

            RuleFor(x => x.LastName)
                 .NotEmpty().WithMessage("Lastname is Empty, please enter the lastname")
                 .Length(1, 50).WithMessage("Lastname length isn't correct");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Please entrer the UserName");

            RuleFor(x => x.Mail)
                .NotEmpty().WithMessage("Enter the mail");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Please enter the password");

        }

    }
}

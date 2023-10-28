using System;
using System.Text.Json.Serialization;
using FluentValidation;

namespace ProgramServer.Application.DTOs
{
    public class SubjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public IEnumerable<EventModel> Events { get; set; }
        public IEnumerable<UserModel> Users { get; set; }
        public DescriptionModel Description { get; set; }
    }

    public class SubjectValidator : AbstractValidator<SubjectModel>
    {
        public SubjectValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Enter the subject name");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Enter the subject code");

            RuleFor(x => x.Description.Location)
                .NotEmpty().WithMessage("Enter the subject location");

        }

    }
}


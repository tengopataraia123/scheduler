using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Application.DTOs
{
    public class SubjectCreateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DescriptionModel Description { get; set; }
    }

    public class SubjectCreateModelValidator : AbstractValidator<SubjectCreateModel>
    {
        public SubjectCreateModelValidator()
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

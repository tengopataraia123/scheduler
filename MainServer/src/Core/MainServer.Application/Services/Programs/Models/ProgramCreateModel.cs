using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Application.Services.Programs.Models
{
    public class ProgramCreateModel
    {
        public string Url { get; set; }
        public string Name { get; set; }
    }

    public class ProgramCreateModelValidator : AbstractValidator<ProgramCreateModel>
    {
        public ProgramCreateModelValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty()
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("Enter the Url");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Enter the Name");

        }

    }
}

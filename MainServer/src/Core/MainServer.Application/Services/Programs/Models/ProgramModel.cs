using System;
using FluentValidation;

namespace MainServer.Application.Services.Programs.Models
{
	public class ProgramModel
	{
        public int Id { get; set; }
        public string? Code { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        // public bool HasBeenActivated { get; set; }
    }

    public class ProgramValidator : AbstractValidator<ProgramModel>
    {
        public ProgramValidator()
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


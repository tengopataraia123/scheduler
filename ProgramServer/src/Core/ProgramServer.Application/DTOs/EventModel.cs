using System;
using FluentValidation;
using ProgramServer.Domain.Attendances;

namespace ProgramServer.Application.DTOs
{
    public class EventModel
    {
        public int Id { get; set; }
        public SubjectModel Subject { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public IEnumerable<AttendanceModel> Attendances { get; set; }
    }

    public class EventValidator : AbstractValidator<EventModel>
    {
        public EventValidator()
        {

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Enter the start date");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("Enter the end date");

        }

    }
}


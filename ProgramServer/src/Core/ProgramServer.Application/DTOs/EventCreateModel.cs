using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Application.DTOs
{
    public class EventCreateModel
    {
        public int SubjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class EventCreateModelValidator : AbstractValidator<EventCreateModel>
    {
        public EventCreateModelValidator()
        {

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Enter the start date");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("Enter the end date");

        }
    }
}

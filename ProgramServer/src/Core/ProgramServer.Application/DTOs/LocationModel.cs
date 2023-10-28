using System.Text.Json.Serialization;
using FluentValidation;
using ProgramServer.Domain.Subjects;

namespace ProgramServer.Application.DTOs
{
    public class LocationModel
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
    }

    public class LocationValidator : AbstractValidator<LocationModel>
    {
        public LocationValidator()
        {

            RuleFor(x => x.Latitude)
                .NotEmpty().WithMessage("Enter the latitude");
        }
    }
}


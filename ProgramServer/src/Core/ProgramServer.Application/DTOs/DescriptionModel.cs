using System;

namespace ProgramServer.Application.DTOs
{
    public class DescriptionModel
    {
        public string Step { get; set; }
        public int Semester { get; set; }
        public int Credit { get; set; }
        public bool IsMandatory { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LocationModel Location { get; set; }
    }
}


using System;
using ProgramServer.Domain.Surveys;

namespace ProgramServer.Application.DTOs
{
    public class SurveyModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Question { get; set; }
    }
}


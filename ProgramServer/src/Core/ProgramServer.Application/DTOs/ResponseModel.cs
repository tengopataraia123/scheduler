using System;
using ProgramServer.Domain.Surveys;

namespace ProgramServer.Application.DTOs
{
    public class ResponseModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string UserMail { get; set; }
        public int SurveyId { get; set; }
        public LikertScale Choice { get; set; }
        public DateTime Timestamp { get; set; }
    }
}


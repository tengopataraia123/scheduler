using System;
using ProgramServer.Domain.Common;

namespace ProgramServer.Domain.Surveys
{
    public class Response : BaseEntity
    {
        public string Subject { get; set; }
        public string UserMail { get; set; }
        public int SurveyId { get; set; }
        public LikertScale Choice { get; set; }
        public DateTime Timestamp { get; set; }
        public Survey Survey { get; set; }
    }
}


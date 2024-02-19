using System;
namespace ProgramServer.Application.DTOs
{
	public class EventGetModel
	{
        public int EventId { get; set; }
        public string SubjectCode { get; set; }
        public int SubjectId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}


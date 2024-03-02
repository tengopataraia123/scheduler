using System;
namespace ProgramServer.Application.DTOs
{
	public class ReccuringEventCreateModel
	{
        public string SubjectCode { get; set; }
        public DateTime RecurringStartDate { get; set; }
        public DateTime RecurringEndDate { get; set; }
        public List<DayOfWeekModel> DaysOfWeek { get; set; }
    }
    public class DayOfWeekModel
    {
        public string Day { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public bool IsChecked { get; set; }
    }
}


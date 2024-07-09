using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Application.DTOs
{
    public class AttendanceReportModel
    {
        public string SubjectName { get; set; }
        public List<UserAttendance> Users { get; set; } = new List<UserAttendance>();
    }

    public class EventAttendance
    {
        public DateTime EventDate { get; set; }
        public double EventTimeLength { get; set; }
        public decimal AttendedTime { get; set; }
    }

    public class UserAttendance
    {
        public string UserName { get; set; }
        public List<EventAttendance> Events { get; set; } = new List<EventAttendance>();
    }
}

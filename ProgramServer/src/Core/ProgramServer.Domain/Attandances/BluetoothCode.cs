using ProgramServer.Domain.Attendances;
using ProgramServer.Domain.Common;
using ProgramServer.Domain.Events;
using ProgramServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Domain.Attandances
{
    public class BluetoothCode : BaseEntity
    {
        public string Code { get; set; }
        public int Count { get; set; }
        public int AttendanceId { get; set; }
        public Attendance Attendance { get; set; }
        public DateTime ActivationTime { get; set; }
    }
}

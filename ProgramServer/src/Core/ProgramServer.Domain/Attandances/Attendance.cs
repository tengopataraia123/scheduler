using System;
using ProgramServer.Domain.Users;
using ProgramServer.Domain.Common;
using ProgramServer.Domain.Events;
using ProgramServer.Domain.Attandances;

namespace ProgramServer.Domain.Attendances
{
    public class Attendance : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}


using System;
using ProgramServer.Domain.Attendances;
using ProgramServer.Domain.Common;
using ProgramServer.Domain.Subjects;

namespace ProgramServer.Domain.Events
{
    public class Event : BaseEntity
    {
        public int SubjectId { get; set; }

        //public Subject Subject { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}


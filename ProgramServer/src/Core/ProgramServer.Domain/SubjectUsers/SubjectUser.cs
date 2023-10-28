using System;
using ProgramServer.Domain.Users;
using ProgramServer.Domain.Subjects;
using ProgramServer.Domain.Common;

namespace ProgramServer.Domain.SubjectUsers
{
    public class SubjectUser : BaseEntity
    {
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}


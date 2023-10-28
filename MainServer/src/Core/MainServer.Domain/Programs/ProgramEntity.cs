using System.ComponentModel.DataAnnotations;
using MainServer.Domain.Common;
using MainServer.Domain.Users;

namespace MainServer.Domain.Programs
{
    public class ProgramEntity : BaseEntity
    {
        public string? Code { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}


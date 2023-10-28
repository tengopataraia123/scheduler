using System.Text.Json.Serialization;
using ProgramServer.Domain.Attendances;
using ProgramServer.Domain.Common;
using ProgramServer.Domain.Roles;
using ProgramServer.Domain.SubjectUsers;

namespace ProgramServer.Domain.Users
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string? MacAddressUser { get; set; }
    }
}


using ProgramServer.Domain.Roles;

namespace ProgramServer.Application.Common.Models
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Email { get; set; }
        public Roles RoleType { get; set; }
    }
}

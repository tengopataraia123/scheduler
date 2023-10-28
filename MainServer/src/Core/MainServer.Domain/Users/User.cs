using MainServer.Domain.Auth.Roles;
using MainServer.Domain.Common;
using MainServer.Domain.Programs;
using System.ComponentModel.DataAnnotations;

namespace MainServer.Domain.Users
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Mail { get; set; }
        [Required]
        [MaxLength(200)]
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        [Required]
        public bool IsBlocked { get; set; }
        public List<ProgramEntity> Program { get; set; }

    }

}
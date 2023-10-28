using System;
using System.ComponentModel.DataAnnotations;
using MainServer.Domain.Common;

namespace MainServer.Domain.Auth.Roles
{
    public class Role : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; }

    }
}


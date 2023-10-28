using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using ProgramServer.Domain.Roles;

namespace ProgramServer.Application.Models.Login
{
    public class SystemUserModel
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
    }
}


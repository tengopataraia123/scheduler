using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MainServer.Domain.Auth.Roles;

namespace MainServer.Application.Models.Login
{
    public class SystemUserModel
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public Role Role { get; set; }
    }
}


using System;
using ProgramServer.Domain.Users;
using ProgramServer.Domain.Common;

namespace ProgramServer.Domain.Roles
{
    public class Role : BaseEntity
    {

        public IEnumerable<User> Users { get; set; }

        public bool IsReceiver { get; set; }

        public bool IsBroadcaster { get; set; }

        public bool IsCreator { get; set; }

        public string RoleName { get; set; }
    }
}

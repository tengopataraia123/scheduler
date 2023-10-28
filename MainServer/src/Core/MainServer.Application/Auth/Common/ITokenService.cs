using MainServer.Application.Models.Login;
using MainServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MainServer.Application.Auth.Common
{
    public interface ITokenService
    {
        SystemUserModel BuildToken(SystemUserModel user);
        string Hash(string password);
        bool Verify(string password, string hashedPassword);
    }
}


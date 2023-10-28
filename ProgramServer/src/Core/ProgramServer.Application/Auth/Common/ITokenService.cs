using ProgramServer.Application.Models.Login;
using ProgramServer.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProgramServer.Application.Auth.Common
{
    public interface ITokenService
    {
        SystemUserModel BuildToken(SystemUserModel user);
        string Hash(string password);
        bool Verify(string password, string hashedPassword);
    }
}


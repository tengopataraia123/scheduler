using MainServer.Application.Auth.Common;
using MainServer.Application.Services.Users.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MainServer.Application.Services.Users.Contracts
{
    public interface IUserLoginService : ILoginService
    {
        Task UserRegistration(UserRegistrationModel userRegistartion);
    }
}

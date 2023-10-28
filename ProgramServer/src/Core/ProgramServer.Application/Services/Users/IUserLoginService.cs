using ProgramServer.Application.Auth.Common;
using ProgramServer.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProgramServer.Application.Services.Users
{
    public interface IUserLoginService : ILoginService
    {
        Task UserRegistration(UserRegistrationModel userRegistartion);
    }
}

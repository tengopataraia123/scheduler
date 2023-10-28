using System;
using MainServer.Application.Services.Users.Models;

namespace MainServer.Application.Services.Users.Contracts
{
    public interface IUserService
    {
        Task BlockUser(int userId);
        Task UnBlockUser(int userId);
        Task<UserModel> GetUserById(int userId);
    }
}


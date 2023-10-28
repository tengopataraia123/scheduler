using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Users
{
    public interface IUserService
    {
        Task<UserModel> FindUser(int id);
        Task<List<UserModel>> GetAllUsers();
        Task DeleteUser(int id);
    }
}


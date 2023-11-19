using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Users
{
    public interface IUserService
    {
        Task Add(UserCreateModel user);
        Task AddUsers(List<UserCreateModel> users);
        Task<List<UserCreateModel>> GetAll();
        //Task Delete(int id);
    }
}


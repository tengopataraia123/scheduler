using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Roles
{
    public interface IRoleService
    {
        Task<RoleModel> FindRole(int id);
        Task<List<RoleModel>> GetAllRoles();
        Task CreateRole(RoleModel role);
        Task DeleteRole(int id);
    }
}


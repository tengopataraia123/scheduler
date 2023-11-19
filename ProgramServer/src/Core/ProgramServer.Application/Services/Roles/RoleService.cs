using System;
using AutoMapper;
using ProgramServer.Application.Exceptions;
using ProgramServer.Application.Repository;
using System.Linq.Expressions;
using ProgramServer.Domain.Roles;
using ProgramServer.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ProgramServer.Application.Services.Roles
{
    public class RoleService : IRoleService
	{
        private readonly IRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRepository<Role> roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task CreateRole(RoleModel roleModel)
        {
            var validator = new RoleValidator();
            var result = validator.Validate(roleModel);

            if (!result.IsValid)
                throw new Exceptions.ValidationException(result.Errors);

            var role = _mapper.Map<Role>(roleModel);
            _roleRepository.Add(role);

            await _roleRepository.SaveAsync();
        }

        //public async Task<RoleModel> FindRole(int id)
        //{
        //    var role = await _roleRepository.Where(o=>o.Id == id).FirstOrDefaultAsync();
        //    if (role == null)
        //        throw new NotFoundException(nameof(Role), id);
        //    return _mapper.Map<RoleModel>(role);
        //}

        //public async Task<List<RoleModel>> GetAllRoles()
        //{
        //    var allroles = await _roleRepository.GetAll().ToListAsync();
        //    return _mapper.Map<List<RoleModel>>(allroles);
        //}

        //public async Task DeleteRole(int id)
        //{
        //    await _roleRepository.Delete(o=>o.Id == id);
        //}
    }
}


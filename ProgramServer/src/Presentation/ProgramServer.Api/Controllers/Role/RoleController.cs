using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Services.Roles;

namespace ProgramServer.Api.Controllers.Role
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost("Create")]
        public async Task<ActionResult> CreateRole([FromBody] RoleModel roleModel)
        {
            await _roleService.CreateRole(roleModel);
            return Ok();
        }

        //[HttpGet("Find/{id}")]
        //public async Task<ActionResult<RoleModel>> FindRole([FromRoute] int id)
        //{
        //    var role = await _roleService.FindRole(id);
        //    return Ok(role);
        //}

        //[HttpGet("GetAllRoles")]
        //public async Task<ActionResult<List<RoleModel>>> GetAllRoles()
        //{
        //    var roles = await _roleService.GetAllRoles();
        //    return Ok(roles);
        //}

        //[HttpDelete("Delete/{id}")]
        //public async Task<ActionResult> DeleteLocation([FromRoute] int id)
        //{
        //    await _roleService.DeleteRole(id);
        //    return Ok();
        //}
    }
}

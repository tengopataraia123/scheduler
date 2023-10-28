using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Services.Bluetooth;
using ProgramServer.Application.Services.Users;

namespace ProgramServer.Api.Controllers.User
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("Find/{id}")]
        public async Task<ActionResult<UserModel>> FindUser([FromRoute] int id)
        {
            var user = await _userService.FindUser(id);
            return Ok(user);
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<List<UserModel>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }

    }
}


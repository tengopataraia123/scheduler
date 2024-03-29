﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Services.Users;

namespace ProgramServer.Api.Controllers.User
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class SchedulerUserController : ControllerBase
    {
        private readonly IUserService _userService;
        public SchedulerUserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Add")]
        public async Task<ActionResult> Add([FromBody] UserCreateModel userModel)
        {
            await _userService.Add(userModel);
            return Ok();
        }

        [HttpPost("AddUsers")]
        public async Task<ActionResult> AddUsers([FromBody] List<UserCreateModel> users)
        {
            await _userService.AddUsers(users);
            return Ok();
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<List<UserGetModel>>> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpDelete("DeleteUser")]
        public async Task<ActionResult> DeleteUser([FromRoute] int userId)
        {
            await _userService.Delete(userId);
            return Ok();
        }
    }
}


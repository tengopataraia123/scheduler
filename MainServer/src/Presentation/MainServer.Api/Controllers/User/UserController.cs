using System;
using MainServer.Application.Services.Users.Contracts;
using MainServer.Application.Services.Users.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainServer.Api.Controllers.User
{
    [Route("[controller]")]
    public class UserController : ApiControllerBase
    {
        public readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "Admin"), HttpPut("Block/{id}")]
        public async Task<ActionResult> BlockUser([FromRoute] int id)
        {
            await _userService.BlockUser(id);
            return Ok();
        }

        [Authorize(Roles = "Admin"), HttpPut("UnBlock/{id}")]
        public async Task<ActionResult> UnBlockUser([FromRoute] int id)
        {
            await _userService.UnBlockUser(id);
            return Ok();
        }

        [Authorize(Roles = "Admin, User"), HttpGet("User/{id}")]
        public async Task<ActionResult<UserModel>> GetUserById([FromRoute] int id)
        {
            var temp = User.Claims;

            if (UserInfo.RoleName.Equals("User"))
                id = UserInfo.Id;

            var user = await _userService.GetUserById(id);
            return Ok(user);
        }
    }
}


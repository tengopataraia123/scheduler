using Microsoft.AspNetCore.Mvc;
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
        public async Task<ActionResult<List<UserCreateModel>>> GetAll()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        //[HttpDelete("Delete/{id}")]
        //public async Task<ActionResult> Delete([FromRoute] int id)
        //{
        //    await _userService.Delete(id);
        //    return Ok();
        //}
    }
}


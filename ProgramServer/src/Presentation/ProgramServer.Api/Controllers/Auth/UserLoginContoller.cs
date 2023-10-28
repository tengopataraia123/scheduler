using ProgramServer.Application.Models.Login;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.Services.Users;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Api.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class UserLoginController : ControllerBase
    {
        public readonly IUserLoginService _userLoginService;

        public UserLoginController(IUserLoginService userLoginService)
        {
            _userLoginService = userLoginService;
        }

        [HttpPost("Registration")]
        public async Task<ActionResult> UserRegistration([FromBody] UserRegistrationModel userModel)
        {
            await _userLoginService.UserRegistration(userModel);
            return Ok();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthenticateResponse>> UserLogin([FromBody] AuthenticateRequest model)
        {
            var result = await _userLoginService.Authenticate(model);
            return Ok(result);
        }
    }
}

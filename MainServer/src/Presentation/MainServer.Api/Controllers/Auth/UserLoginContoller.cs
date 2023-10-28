using MainServer.Application.Models.Login;
using MainServer.Application.Services.Users.Contracts;
using MainServer.Application.Services.Users.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainServer.Api.Controllers.Auth
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

        [HttpPost("UserRegistration")]
        public async Task<ActionResult> UserRegistration([FromBody] UserRegistrationModel userModel)
        {
            await _userLoginService.UserRegistration(userModel);
            return Ok();
        }

        [HttpPost("UserLogin")]
        public async Task<ActionResult<AuthenticateResponse>> UserLogin([FromBody] AuthenticateRequest model)
        {
            var result = await _userLoginService.Authenticate(model);
            return Ok(result);
        }
    }
}

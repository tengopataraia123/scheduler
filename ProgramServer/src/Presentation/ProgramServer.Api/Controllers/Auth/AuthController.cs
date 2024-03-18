using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.Services.Auth;
using ProgramServer.Domain.Auth;

namespace ProgramServer.Api.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authService;
    private readonly string adminUsername,adminPassword;
    public AuthController(IAuthService authService,IConfiguration configuration)
    {
        _authService = authService;
        adminUsername = configuration.GetValue<string>("AdminUsername") ?? string.Empty;
        adminPassword = configuration.GetValue<string>("AdminPassword") ?? string.Empty;
    }
    
    [HttpGet]
    public async Task<IActionResult> Authenticate(string token)
    {

        var internalToken = await _authService.GenerateToken(token);
        
        return Ok(internalToken);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> AuthenticateAdmin([FromBody] AuthDTO auth)
    {
        if (auth.Mail != adminUsername && auth.Password != adminPassword)
            return Forbid();
        var token = _authService.GenerateAdminToken();
        return Ok(token);
    }
}
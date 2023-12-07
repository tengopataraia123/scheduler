using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.Services.Auth;

namespace ProgramServer.Api.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Authenticate(string token)
    {

        var internalToken = await _authService.GenerateToken(token);
        
        return Ok(internalToken);
    }
}
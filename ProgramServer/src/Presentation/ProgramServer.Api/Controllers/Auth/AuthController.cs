using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ProgramServer.Api.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Authenticate(string token)
    {
        
        
        
        return Ok();
    }
}
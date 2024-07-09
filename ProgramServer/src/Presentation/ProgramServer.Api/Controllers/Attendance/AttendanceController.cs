using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.Services.Attendances;
using ProgramServer.Application.Services.Auth;
using ProgramServer.Domain.Auth;

namespace ProgramServer.Api.Controllers.Auth;

[ApiController]
[Route("[controller]")]
public class AttendanceController : ControllerBase
{

    private readonly IAttendanceService _attendanceService;
    public AttendanceController(IAttendanceService attendanceService)
    {
        _attendanceService = attendanceService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAttendances()
    {

        var result = await _attendanceService.GetAttendances();
        
        return Ok(result);
    }
}
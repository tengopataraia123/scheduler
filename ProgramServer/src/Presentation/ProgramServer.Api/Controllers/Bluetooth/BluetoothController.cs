using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.Services.Bluetooth;

namespace ProgramServer.Api.Controllers.Bluetooth;

//[Authorize]
[Route("[controller]")]
public class BluetoothController : ControllerBase
{

    private readonly int _userId;
    private readonly IBluetoothService _bluetoothService;

    public BluetoothController(IBluetoothService bluetoothService)
    {
        _userId = User.Claims.Where(o=>o.Type == ClaimTypes.NameIdentifier).Select(o=>Convert.ToInt32(o.Value)).FirstOrDefault();
        _bluetoothService = bluetoothService;
    }
    
    //[Authorize]
    [HttpGet("[action]")]
    public async Task<IActionResult> GetBluetoothCodes(int userId)
    {
        //var result = await _bluetoothService.GetBluetoothCodes(_userId);
        var result = await _bluetoothService.GetBluetoothCodes(userId);
        return Ok(result);
    }
    
    //[Authorize]
    [HttpPost("[action]")]
    public async Task<IActionResult> ScannedBluetoothCodes([FromBody] List<string> codes)
    {
        await _bluetoothService.ScannedBluetoothCodes(codes);
        return Ok();
    }
}
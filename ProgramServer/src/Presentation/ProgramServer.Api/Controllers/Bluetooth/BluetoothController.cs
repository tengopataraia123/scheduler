using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.Common.Models;
using ProgramServer.Application.Services.Bluetooth;

namespace ProgramServer.Api.Controllers.Bluetooth;

[Route("[controller]")]
[Authorize]
public class BluetoothController : ApiControllerBase
{

    private readonly IBluetoothService _bluetoothService;
    private readonly int _userId;

    public BluetoothController(IBluetoothService bluetoothService)
    {
        _userId = User.Claims.Where(o=>o.Type == ClaimTypes.NameIdentifier).Select(o=>Convert.ToInt32(o.Value)).FirstOrDefault();
        _bluetoothService = bluetoothService;
    }
    
    [HttpGet("[action]")]
    public async Task<IActionResult> GetBluetoothCodes()
    {
        //var result = await _bluetoothService.GetBluetoothCodes(_userId);
        var result = await _bluetoothService.GetBluetoothCodes(CurrentUser.Id);
        return Ok(result);
    }
    
    [HttpPost("[action]")]
    public async Task<IActionResult> ScannedBluetoothCodes([FromBody] List<string> codes)
    {
        await _bluetoothService.ScannedBluetoothCodes(_userId,codes);
        return Ok();
    }
}
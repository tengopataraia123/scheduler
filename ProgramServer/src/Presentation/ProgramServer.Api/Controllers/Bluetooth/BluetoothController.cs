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

    public BluetoothController(IBluetoothService bluetoothService)
    {
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
        await _bluetoothService.ScannedBluetoothCodes(CurrentUser.Id, codes);
        return Ok();
    }
}
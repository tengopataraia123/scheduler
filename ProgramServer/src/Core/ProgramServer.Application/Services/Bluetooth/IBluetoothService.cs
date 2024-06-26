using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Bluetooth;

public interface IBluetoothService
{
    Task<List<AttendanceModel>> GetBluetoothCodes(int userId);
    Task ScannedBluetoothCodes(int userId,List<string> codes);
}
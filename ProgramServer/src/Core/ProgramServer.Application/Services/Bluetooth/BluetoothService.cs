using Microsoft.EntityFrameworkCore;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Repository;
using ProgramServer.Domain.Attandances;
using ProgramServer.Domain.Attendances;

namespace ProgramServer.Application.Services.Bluetooth;

public class BluetoothService : IBluetoothService
{
    private readonly IRepository<BluetoothCode> _bluetoothCodeRepository;
    private readonly IRepository<Attendance> _attendanceRepository;
    public BluetoothService(IRepository<BluetoothCode> bluetoothCodeRepository, IRepository<Attendance> attendanceRepository)
    {
        _bluetoothCodeRepository = bluetoothCodeRepository;
        _attendanceRepository = attendanceRepository;
    }
    public async Task<List<AttendanceModel>> GetBluetoothCodes(int userId)
    {
        var attendancesQuery = from attendance in _attendanceRepository.GetAll()
            join bluetoothCode in _bluetoothCodeRepository.GetAll()
                on attendance.Id equals bluetoothCode.AttendanceId
            where attendance.UserId == userId
            select new { attendance, bluetoothCode };
        var attendances = await attendancesQuery.GroupBy(o => o.attendance.Id)
            .Select(o => new AttendanceModel
            {
                EventId = o.First().attendance.Id,
                UserId = o.First().attendance.UserId,
                BluetoothCodes = o.Select(x => new BluetoothCodeModel
                {
                    Code = x.bluetoothCode.Code
                })
            }).ToListAsync();

        return attendances;
    }

    public async Task ScannedBluetoothCodes(List<string> codes)
    {
        var codesFromDb = await _bluetoothCodeRepository.Where(o => codes.Contains((o.Code))).ToListAsync();
        
        codesFromDb.ForEach(o=>o.Count++);

        await _bluetoothCodeRepository.SaveAsync();
    }
}
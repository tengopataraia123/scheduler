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
    private readonly IRepository<BluetoothLog> _bluetoothLogsRepository;
    public BluetoothService(IRepository<BluetoothCode> bluetoothCodeRepository,
        IRepository<Attendance> attendanceRepository,
        IRepository<BluetoothLog> bluetoothLogsRepository)
    {
        _bluetoothCodeRepository = bluetoothCodeRepository;
        _attendanceRepository = attendanceRepository;
        _bluetoothLogsRepository = bluetoothLogsRepository;
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
                EventId = o.First().attendance.EventId,
                UserId = o.First().attendance.UserId,
                BluetoothCodes = o.Select(x => new BluetoothCodeModel
                {
                    Code = x.bluetoothCode.Code,
                    ActivateTime = x.bluetoothCode.ActivationTime
                })
            }).ToListAsync();

        return attendances;
    }

    public async Task ScannedBluetoothCodes(int userId, List<string> codes)
    {
        foreach(var code in codes)
        {
            _bluetoothLogsRepository.Add(new BluetoothLog
            {
                ScanDate = DateTime.Now,
                BluetoothCode = code,
                ScannedById = userId
            });
        }

        var codesFromDb = await _bluetoothCodeRepository.Where(o => codes.Contains((o.Code))).ToListAsync();
        
        codesFromDb.ForEach(o=>o.Count++);

        await _bluetoothCodeRepository.SaveAsync();
        await _bluetoothLogsRepository.SaveAsync();
    }
}
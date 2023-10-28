using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Attendances
{
    public interface IAttendanceService
    {
        Task<AttendanceModel> FindAttendance(int id);
        Task<List<AttendanceModel>> GetAllAttendances();
    }
}


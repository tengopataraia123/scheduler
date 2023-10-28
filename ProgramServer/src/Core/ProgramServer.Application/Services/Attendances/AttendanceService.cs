using System;
using AutoMapper;
using ProgramServer.Application.Exceptions;
using ProgramServer.Application.Repository;
using System.Linq.Expressions;
using ProgramServer.Domain.Attendances;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Services.Attendances;
using Microsoft.EntityFrameworkCore;

namespace ProgramServer.Application.Services.AttendanceService
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IMapper _mapper;

        public AttendanceService(IRepository<Attendance> locationRepository, IMapper mapper)
        {
            _attendanceRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<AttendanceModel> FindAttendance(int id)
        {
            var attendance = await _attendanceRepository.Where(o=>o.Id == id).FirstOrDefaultAsync();
            if (attendance == null)
                throw new NotFoundException(nameof(Attendance), id);
            return _mapper.Map<AttendanceModel>(attendance);
        }

        public async Task<List<AttendanceModel>> GetAllAttendances()
        {
            var allattendance = await _attendanceRepository.GetAll().ToListAsync();
            return _mapper.Map<List<AttendanceModel>>(allattendance);
        }
    }
}


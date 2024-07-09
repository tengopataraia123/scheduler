using System;
using AutoMapper;
using ProgramServer.Application.Exceptions;
using ProgramServer.Application.Repository;
using System.Linq.Expressions;
using ProgramServer.Domain.Attendances;
using ProgramServer.Application.DTOs;
using ProgramServer.Application.Services.Attendances;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Domain.Attandances;
using ProgramServer.Domain.Subjects;

namespace ProgramServer.Application.Services.AttendanceService
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IRepository<BluetoothCode> _bluetoothCodeRepository;
        private readonly IMapper _mapper;

        private readonly IRepository<Subject> _subjectRepository;

        public AttendanceService(IRepository<Attendance> locationRepository,
            IRepository<BluetoothCode> bluetoothRepository,
            IRepository<Subject> subjectRepository,
            IMapper mapper)
        {
            _attendanceRepository = locationRepository;
            _bluetoothCodeRepository = bluetoothRepository;
            _subjectRepository = subjectRepository;
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

        public async Task<List<AttendanceReportModel>> GetAttendances()
        {
            var result = new List<AttendanceReportModel>();

            var subjects = await _subjectRepository.GetAll()
                .Include(o=>o.SubjectUsers).ThenInclude(o=>o.User)
                .Include(o=>o.Events)
                .ToListAsync();

            var attendances = await _bluetoothCodeRepository.Where(o=>o.Count > 0)
                .Include(o => o.Attendance).ToListAsync();

            foreach (var subject in subjects)
            {
                var subjectAttendance = new AttendanceReportModel();
                subjectAttendance.SubjectName = subject.Name;

                foreach (var subjectUser in subject.SubjectUsers)
                {
                    var userAttendance = new UserAttendance();
                    userAttendance.UserName = subjectUser.User.FirstName + " " + subjectUser.User.LastName;

                    foreach(var evnt in subject.Events)
                    {
                        var eventAttendance = new EventAttendance();
                        eventAttendance.EventDate = evnt.StartDate;
                        eventAttendance.EventTimeLength = (evnt.EndDate - evnt.StartDate).TotalSeconds;

                        var userEventAttendances = attendances.Where(o=> o.Attendance.EventId == evnt.Id && o.Attendance.UserId == subjectUser.UserId).ToList();
                        eventAttendance.AttendedTime = 20 * 60 * (userEventAttendances.Count-1);

                        userAttendance.Events.Add(eventAttendance);
                    }
                    subjectAttendance.Users.Add(userAttendance);
                }
                result.Add(subjectAttendance);
            }
            return result;
        }
    }
}


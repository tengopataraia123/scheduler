using System;
using AutoMapper;
using ProgramServer.Application.Exceptions;
using ProgramServer.Application.Repository;
using System.Linq.Expressions;
using ProgramServer.Domain.Events;
using ProgramServer.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Domain.Subjects;
using ProgramServer.Domain.Attandances;
using ProgramServer.Domain.SubjectUsers;
using System.Security.Cryptography;
using System.Text;
using ProgramServer.Domain.Attendances;
using ProgramServer.Domain.Users;
using System.Diagnostics.Tracing;

namespace ProgramServer.Application.Services.Events
{
    public class EventService: IEventService
    { 
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<SubjectUser> _subjectUserRepository;
        private readonly IRepository<BluetoothCode> _bluetoothCodeRepository;
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IMapper _mapper;

        public EventService(IRepository<Event> eventRepository,
            IRepository<Subject> subjectRepository,
            IRepository<User> userRepository,
            IRepository<SubjectUser> subjectUserRepository,
            IRepository<BluetoothCode> bluetoothCodeRepository,
            IRepository<Attendance> attendanceRepository,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _subjectRepository = subjectRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _subjectUserRepository = subjectUserRepository;
            _bluetoothCodeRepository = bluetoothCodeRepository;
            _attendanceRepository = attendanceRepository;
        }

        public async Task<int> Add(EventCreateModel eventModel)
        {
            var validator = new EventCreateModelValidator();
            var result = validator.Validate(eventModel);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);
            
            var subject = await _subjectRepository.Where(o => o.Code == eventModel.SubjectCode).FirstOrDefaultAsync();
            eventModel.SubjectId = subject.Id;
            var eventEntity = new Event
            {
                SubjectId = eventModel.SubjectId,
                StartDate = EnsureUtc(eventModel.StartDate),
                EndDate = EnsureUtc(eventModel.EndDate),
            };
            var subjectUsers = await _subjectRepository.Where(o => o.Id == subject.Id)
                .Include(o => o.SubjectUsers)
                .ThenInclude(o => o.User).FirstOrDefaultAsync();
 
            var users = subjectUsers.SubjectUsers.Select(o => o.User).ToList();

            _eventRepository.Add(eventEntity);
            await _eventRepository.SaveAsync();
            return eventEntity.Id;
        }

        public async Task AddEvents(List<EventCreateModel> events)
        {
            var users = await _subjectUserRepository.Where(o => o.SubjectId == events.First().SubjectId).Include(o => o.User).Select(o => o.User).ToListAsync();
            var validator = new EventCreateModelValidator();
            foreach (var eventModel in events)
            {
                var result = validator.Validate(eventModel);

                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
                var eventId = await Add(eventModel);

                GenerateBluetoothCodes(eventId,eventModel.StartDate,eventModel.EndDate, users);
            }
                            
            await _bluetoothCodeRepository.SaveAsync();
        }

        public Task AddRecurringEvents(ReccuringEventCreateModel reccuringEvent)
        {
            throw new NotImplementedException();
        }

        public async Task<List<EventGetModel>> GetAll()
        {
            var allevents = await _eventRepository.GetAll().ToListAsync();
            return _mapper.Map<List<EventGetModel>>(allevents);
        }

        private DateTime EnsureUtc(DateTime dateTime)
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
        public async Task DeleteEvents(List<int> eventIds)
        {
            await _eventRepository.Delete(o=> eventIds.Contains(o.Id));
        }

        private void GenerateBluetoothCodes(int eventId,DateTime startDate, DateTime endTime, List<User> users)
        {
            var bluetoothCodes = new List<BluetoothCode>();
            
            foreach (var user in users)
            {
                var code = GenerateUniqueCode(bluetoothCodes);
                var attendance = new Attendance
                {
                    UserId = user.Id,
                    EventId = eventId,
                }; 
                
                for (var time = startDate; time <= endTime; time = time.AddMinutes(20))
                {
                    bluetoothCodes.Add(new BluetoothCode
                    {
                        Code = code,
                        ActivationTime = time,
                        AttendanceId = attendance.Id
                    });
                }
                _attendanceRepository.Add(attendance);
                _bluetoothCodeRepository.AddRange(bluetoothCodes);
            }
        }

        private string GenerateUniqueCode(List<BluetoothCode> bluetoothCodes, int counter = 0)
        {
            if (counter == 10)
                throw new Exception("Can't Generate bluetooth codes");

            char[] symbols = "1234567890".ToArray();

            var result = new StringBuilder(5);

            for (var i = 0; i < 8; i++)
            {
                var index = RandomNumberGenerator.GetInt32(symbols.Length);
                result.Append(symbols[index]);
            }

            if (bluetoothCodes.Any(o => o.Code == result.ToString()))
                return GenerateUniqueCode(bluetoothCodes, counter++);

            return result.ToString();
        }
    }
}


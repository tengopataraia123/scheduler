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
using Microsoft.Extensions.Logging;

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
            var validator = new EventCreateModelValidator();
            foreach (var eventModel in events)
            {
                var result = validator.Validate(eventModel);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }

            var subjectCodes = events.Select(e => e.SubjectCode).Distinct().ToList();
            var subjects = await _subjectRepository.Where(s => subjectCodes.Contains(s.Code)).ToListAsync();

            foreach (var eventModel in events)
            {
                var subject = subjects.FirstOrDefault(s => s.Code == eventModel.SubjectCode);
                if (subject != null)
                {
                    eventModel.SubjectId = subject.Id;
                }
                else
                {
                    throw new Exception($"Subject with code: {eventModel.SubjectCode}, doesn't exist!");
                }
            }
            var subjectId = events.First().SubjectId;
            var users = await _subjectUserRepository.Where(o => o.SubjectId == subjectId).Include(o => o.User).Select(o => o.User).ToListAsync();

            foreach (var eventModel in events)
            {
                var eventId = await Add(eventModel);
                await GenerateBluetoothCodes(eventId, eventModel.StartDate, eventModel.EndDate, users);
            }

            await _bluetoothCodeRepository.SaveAsync();
        }

        public async Task AddRecurringEvents(ReccuringEventCreateModel recurringEventModel)
        {
            var activeDays = recurringEventModel.DaysOfWeek.Where(d => d.IsChecked).ToList();

            foreach (var dayOfWeek in activeDays)
            {
                if (!DayOfWeekMapping.TryGetValue(dayOfWeek.Day, out var englishDayName))
                {
                    throw new Exception($"Invalid day of week: {dayOfWeek.Day}");
                }
                if (!Enum.TryParse<DayOfWeek>(englishDayName, true, out var dayOfWeekEnum))
                {
                    throw new Exception($"Invalid day of week: {dayOfWeek.Day}");
                }

                var currentDate = recurringEventModel.RecurringStartDate;
                while (currentDate <= recurringEventModel.RecurringEndDate)
                {
                    if (currentDate.DayOfWeek == dayOfWeekEnum)
                    {
                        var startTimeSpan = TimeSpan.Parse(dayOfWeek.StartHour);
                        var endTimeSpan = TimeSpan.Parse(dayOfWeek.EndHour);

                        var startDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).Add(startTimeSpan);
                        var endDateTime = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day).Add(endTimeSpan);

                        var eventCreateModel = new EventCreateModel
                        {
                            SubjectCode = recurringEventModel.SubjectCode,
                            StartDate = startDateTime,
                            EndDate = endDateTime,
                        };

                        var eventId = await Add(eventCreateModel);
                        var subjects = await _subjectRepository.Where(s => eventCreateModel.SubjectCode == s.Code).ToListAsync();
                        var subject = subjects.FirstOrDefault(s => s.Code == eventCreateModel.SubjectCode);
                        if (subject != null)
                        {
                            eventCreateModel.SubjectId = subject.Id;
                        }
                        else
                        {
                            throw new Exception($"Subject with code: {eventCreateModel.SubjectCode}, doesn't exist!");
                        }
                        var users = await _subjectUserRepository.Where(o => o.SubjectId == subject.Id).Include(o => o.User).Select(o => o.User).ToListAsync();

                        await GenerateBluetoothCodes(eventId, eventCreateModel.StartDate, eventCreateModel.EndDate, users);
                    }

                    currentDate = currentDate.AddDays(1);
                }
            }
        }

        static readonly Dictionary<string, string> DayOfWeekMapping = new Dictionary<string, string>
        {
            { "ორშაბათი", "Monday" },
            { "სამშაბათი", "Tuesday" },
            { "ოთხშაბათი", "Wednesday" },
            { "ხუთშაბათი", "Thursday" },
            { "პარასკევი", "Friday" },
            { "შაბათი", "Saturday" },
            { "კვირა", "Sunday" }
        };

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

        private async Task GenerateBluetoothCodes(int eventId, DateTime startDate, DateTime endTime, List<User> users)
        {
            foreach (var user in users)
            {
                var attendance = await CreateAttendanceAsync(user.Id, eventId);

                var existingBluetoothCodes = await _bluetoothCodeRepository.GetAll().Select(o => o.Code).ToListAsync();
                
                var bluetoothCodes = new List<BluetoothCode>();

                for (var time = EnsureUtc(startDate); time <= EnsureUtc(endTime); time = time.AddMinutes(20))
                {
                    var code = GenerateUniqueCode(existingBluetoothCodes);
                    bluetoothCodes.Add(new BluetoothCode
                    {
                        Code = code,
                        ActivationTime = time, 
                        AttendanceId = attendance.Id 
                    });
                }

                _bluetoothCodeRepository.AddRange(bluetoothCodes);
            }
        }


        private string GenerateUniqueCode(List<string> bluetoothCodes, int counter = 0)
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

            if (bluetoothCodes.Any(o => o == result.ToString()))
                return GenerateUniqueCode(bluetoothCodes, counter++);

            return result.ToString();
        }

        private async Task<Attendance> CreateAttendanceAsync(int userId, int eventId)
        {
            var attendance = new Attendance
            {
                UserId = userId,
                EventId = eventId,
            };

            _attendanceRepository.Add(attendance);
            await _attendanceRepository.SaveAsync(); 

            return attendance; 
        }

    }
}


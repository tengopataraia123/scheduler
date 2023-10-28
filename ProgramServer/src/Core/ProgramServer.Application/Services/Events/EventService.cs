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

namespace ProgramServer.Application.Services.Events
{
    public class EventService: IEventService
    { 
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IRepository<BluetoothCode> _bluetoothCodeRepository;
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IMapper _mapper;

        public EventService(IRepository<Event> eventRepository,
            IRepository<Subject> subjectRepository,
            IRepository<BluetoothCode> bluetoothCodeRepository,
            IRepository<Attendance> attendanceRepository,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _subjectRepository = subjectRepository;
            _bluetoothCodeRepository = bluetoothCodeRepository;
            _attendanceRepository = attendanceRepository;
            _mapper = mapper;
        }

        public async Task<EventModel> FindEvent(int id)
        {
            var eventResult = await _eventRepository.Where(o=>o.Id == id).FirstOrDefaultAsync();
            if (eventResult == null)
                throw new NotFoundException(nameof(Event), id);

            return _mapper.Map<EventModel>(eventResult);
        }

        public async Task<List<EventModel>> GetAllEvents()
        {
            var allevents = await _eventRepository.GetAll().ToListAsync();
            return _mapper.Map<List<EventModel>>(allevents);
        }

        public async Task CreateEvent(EventCreateModel eventModel)
        {
            var validator = new EventCreateModelValidator();
            var result = validator.Validate(eventModel);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var eventEntity = _mapper.Map<Event>(eventModel);


            var subjectUsers = await _subjectRepository.Where(o => o.Id == eventModel.SubjectId)
                .Include(o => o.SubjectUsers)
                .ThenInclude(o => o.User).FirstOrDefaultAsync();

            var users = subjectUsers.SubjectUsers.Select(o => o.User).ToList();

            GenerateBluetoothCodes(eventEntity, users);

            _eventRepository.Add(eventEntity);

            await _eventRepository.SaveAsync();
        }

        private void GenerateBluetoothCodes(Event eventEntity, IEnumerable<User> users)
        {
            var bluetoothCodes = new List<BluetoothCode>();

            foreach(var user in users)
            {
                var attendance = new Attendance()
                {
                    Event = eventEntity,
                    User = user
                };

                for (var time = eventEntity.StartDate; time <= eventEntity.EndDate;time =  time.AddMinutes(20)) {
                    var code = GenerateUniqueCode(bluetoothCodes);
                    bluetoothCodes.Add(new BluetoothCode
                    {
                        Code = code,
                        ActivationTime = time,
                        Attendance = attendance
                    });
                }

                _attendanceRepository.Add(attendance);
            }
            _bluetoothCodeRepository.AddRange(bluetoothCodes);
        }

        private string GenerateUniqueCode(List<BluetoothCode> bluetoothCodes,int counter = 0)
        {
            if (counter == 10)
                throw new Exception("Can't Generate bluetooth codes");

            char[] symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToArray();

            var result = new StringBuilder(5);

            for (var i = 0; i < 5; i++)
            {
                var index = RandomNumberGenerator.GetInt32(symbols.Length);
                result.Append(symbols[index]);
            }

            if(bluetoothCodes.Any(o=>o.Code == result.ToString()))
                return GenerateUniqueCode(bluetoothCodes,counter++);

            return result.ToString();
        }

        public async Task DeleteEvent(int id)
        {
            await _eventRepository.Delete(o=>o.Id == id);
        }
    }
}


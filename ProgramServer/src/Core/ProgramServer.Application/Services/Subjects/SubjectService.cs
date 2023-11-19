

using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.Exceptions;
using ProgramServer.Application.Repository;
using ProgramServer.Domain.Users;
using ProgramServer.Domain.Subjects;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Application.DTOs;
using ProgramServer.Domain.Attandances;
using ProgramServer.Domain.Attendances;
using ProgramServer.Domain.Events;
using ProgramServer.Domain.SubjectUsers;

namespace ProgramServer.Application.Services.Subjects
{
    public class SubjectService : ISubjectService
    {
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IRepository<SubjectUser> _subjectUserRepository;
        private readonly IRepository<BluetoothCode> _bluetoothCodeRepository;
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public SubjectService(IRepository<Subject> subjectRepository,
            IRepository<SubjectUser> subjectUserRepository,
            IRepository<BluetoothCode> bluetoothCodeRepository,
            IRepository<Attendance> attendanceRepository,
            IRepository<Event> eventRepository,
            IRepository<User> userRepository,
            IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _subjectUserRepository = subjectUserRepository;
            _bluetoothCodeRepository = bluetoothCodeRepository;
            _attendanceRepository = attendanceRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task CreateSubject(SubjectCreateModel subjectModel)
        {
            var validator = new SubjectCreateModelValidator();
            var result = validator.Validate(subjectModel);

            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var subject = _mapper.Map<Subject>(subjectModel);

            _subjectRepository.Add(subject);

            await _subjectRepository.SaveAsync();
        }

        public async Task AddSubjectUsers(List<SubjectUserModel> subjectUsers)
        {
            foreach (var subjectUser in subjectUsers)
            {
                var entity = _mapper.Map<SubjectUser>(subjectUser);

                _subjectUserRepository.Add(entity);
                await _subjectUserRepository.SaveAsync();

                var events = await _eventRepository.Where(o => o.SubjectId == subjectUser.SubjectId).ToListAsync();
                var user = await _userRepository.Where(o => o.Id == subjectUser.UserId).FirstOrDefaultAsync();
                
                GenerateBluetoothCodes(events,user);
                await _bluetoothCodeRepository.SaveAsync();
            }
        }

        public async Task<List<SubjectCreateModel>> GetSubjectsByUserId(int userId)
        {
            var subjects = await _subjectRepository.GetAll().Where(x => x.SubjectUsers.Any(s => s.UserId == userId))
                .Include(s => s.Events)
                .Include(a => a.SubjectUsers)
                .ToListAsync();

            if (subjects == null)
            {
                throw new NotFoundException(nameof(Subject), userId);
            }

            return _mapper.Map<List<SubjectCreateModel>>(subjects);
        }

        public async Task<List<SubjectCreateModel>> GetAllSubjects()
        {
            var allusers = await _subjectRepository.GetAll().ToListAsync();
            return _mapper.Map<List<SubjectCreateModel>>(allusers);
        }

        //public async Task<SubjectModel> FindSubject(int id)
        //{
        //    var subject = await _subjectRepository.GetAll().Where(x=>x.Id == id).FirstOrDefaultAsync();

        //    if (subject == null)
        //        throw new NotFoundException(nameof(Subject), id);
        //    return _mapper.Map<SubjectModel>(subject);
        //}

        //public async Task DeleteSubject(int id)
        //{
        //    await _subjectRepository.Delete(o=>o.Id  == id);
        //}
        
        
        private void GenerateBluetoothCodes(IEnumerable<Event> eventEntities, User user)
        {
            var bluetoothCodes = new List<BluetoothCode>();

            foreach(var eventEntity in eventEntities)
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
    }
}


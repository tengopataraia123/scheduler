

using System.Security.Cryptography;
using System.Text;
using AutoMapper;
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
                var subject = await _subjectRepository.Where(s => s.Code == subjectUser.SubjectCode).FirstOrDefaultAsync();
                if (subject == null)
                {
                    continue; 
                }
                subjectUser.SubjectId = subject.Id;

                var userEntity = await _userRepository.Where(u => u.Email == subjectUser.UserEmail).FirstOrDefaultAsync();
                if (userEntity == null)
                {
                    continue; 
                }
                subjectUser.UserId = userEntity.Id;

                var existingSubjectUser = await _subjectUserRepository
                    .Where(su => su.UserId == subjectUser.UserId && su.SubjectId == subjectUser.SubjectId)
                    .FirstOrDefaultAsync();

                if (existingSubjectUser != null)
                {
                    continue;
                }

                var entity = _mapper.Map<SubjectUser>(subjectUser);
                _subjectUserRepository.Add(entity);
                await _subjectUserRepository.SaveAsync();
            }
        }

        public async Task<List<SubjectUserModel>> GetUsersBySubjectCode(string subjectCode)
        {
            var subjects = await _subjectRepository.Where(x => x.Code == subjectCode)
                .Include(a => a.SubjectUsers)
                .ThenInclude(u=>u.User)
                .ToListAsync();

            if (subjects == null)
            {
                throw new NotFoundException(nameof(Subject), subjectCode);
            }

            var subjectUsers = subjects.SelectMany(s => s.SubjectUsers).ToList();

            return _mapper.Map<List<SubjectUserModel>>(subjectUsers);
        }

        public async Task<List<SubjectGetModel>> GetAllSubjects()
        {
            var allusers = await _subjectRepository.GetAll().ToListAsync();
            return _mapper.Map<List<SubjectGetModel>>(allusers);
        }

        public Task DeleteSubject(string subjectCode)
        {
            throw new NotImplementedException();
        }

    }
}


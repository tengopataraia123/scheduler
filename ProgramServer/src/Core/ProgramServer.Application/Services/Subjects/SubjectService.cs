

using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProgramServer.Application.Exceptions;
using ProgramServer.Application.Repository;
using ProgramServer.Domain.Users;
using ProgramServer.Domain.Subjects;
using Microsoft.EntityFrameworkCore;
using ProgramServer.Application.DTOs;
using ProgramServer.Domain.SubjectUsers;

namespace ProgramServer.Application.Services.Subjects
{
    public class SubjectService : ISubjectService
    {
        private readonly IRepository<Subject> _subjectRepository;
        private readonly IRepository<SubjectUser> _subjectUserRepository;
        private readonly IMapper _mapper;

        public SubjectService(IRepository<Subject> subjectRepository,
            IRepository<SubjectUser> subjectUserRepository,
            IMapper mapper)
        {
            _subjectRepository = subjectRepository;
            _subjectUserRepository = subjectUserRepository;
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
    }
}


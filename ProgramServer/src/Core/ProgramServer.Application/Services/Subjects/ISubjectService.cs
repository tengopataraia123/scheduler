using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Subjects
{
    public interface ISubjectService
    {
        Task CreateSubject(SubjectCreateModel subject);
        Task AddSubjectUsers(List<SubjectUserModel> subjectUser);
        Task<List<SubjectGetModel>> GetSubjectsByUserId(int id);
        Task<List<SubjectGetModel>> GetAllSubjects();
        //Task<SubjectModel> FindSubject(int id);
        //Task DeleteSubject(int id);
    }
}


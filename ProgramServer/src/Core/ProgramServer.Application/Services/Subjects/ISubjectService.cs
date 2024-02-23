using System;
using ProgramServer.Application.DTOs;

namespace ProgramServer.Application.Services.Subjects
{
    public interface ISubjectService
    {
        Task CreateSubject(SubjectCreateModel subject);
        Task AddSubjectUsers(List<SubjectUserModel> subjectUser);
        Task<List<SubjectGetModel>> GetAllSubjects();
        Task<List<SubjectUserModel>> GetUsersBySubjectCode(string subjectCode);
        Task DeleteSubject(string subjectCode);
    }
}

